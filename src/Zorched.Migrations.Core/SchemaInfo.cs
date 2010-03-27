// Copyright 2008, 2009 Geoff Lane <geoff@zorched.net>
// 
// This file is part of Zorched Migrations a SQL Schema Migration framework for .NET.
// 
// Zorched Migrations is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Zorched Migrations is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Data;
using Zorched.Migrations.Framework;
using Zorched.Migrations.Framework.Data;
using Zorched.Migrations.Framework.Inspection;
using Zorched.Migrations.Framework.Schema;

namespace Zorched.Migrations.Core
{
    /// <summary>
    /// Manages all of the interactions with the SchemaInfo table
    /// that records what Migrations have been run against the database.
    /// </summary>
    public class SchemaInfo : ISchemaInfo
    {
        public const string SCHEMA_VERSION_TABLE = "SchemaInfo";

        public readonly Column ASSEMBLY_COLUMN = new Column
                                                     {
                                                         Name = "Assembly",
                                                         DbType = DbType.String,
                                                         Size = 255,
                                                         Property = ColumnProperty.NotNull,
                                                         DefaultValue = "('FIXME')"
                                                     };

        public readonly Column TYPE_COLUMN = new Column
                                                 {
                                                     Name = "Type",
                                                     DbType = DbType.String,
                                                     Size = 255,
                                                     Property = ColumnProperty.NotNull,
                                                     DefaultValue = "('FIXME')"
                                                 };

        public readonly Column APPLIED_ON_COLUMN = new Column
                                                       {
                                                           Name = "AppliedOn",
                                                           DbType = DbType.DateTime,
                                                           Property = ColumnProperty.NotNull,
                                                           DefaultValue = "(getdate())"
                                                       };

        public SchemaInfo(IDriver driver)
        {
            Driver = driver;
        }

        public IDriver Driver { get; set; }

        /// <summary>
        /// Creates or updates the SchemaInfo table so that it has the proper schema.
        /// </summary>
        public void EnsureSchemaTable()
        {
            if (! SchemaInfoTableExists())
            {
                CreateSchemaTable();
            }

            EnsureColumn(ASSEMBLY_COLUMN);
            EnsureColumn(TYPE_COLUMN);
            EnsureColumn(APPLIED_ON_COLUMN);
        }

        /// <summary>
        /// Checks if a column exists and creates it if it doesn't.
        /// </summary>
        /// <param name="c">The column definition to check and create.</param>
        public void EnsureColumn(Column c)
        {
            if (!Driver.Inspect<IColumnExistsOperation>(op =>
                                                            {
                                                                op.TableName = SCHEMA_VERSION_TABLE;
                                                                op.ColumnName = c.Name;
                                                            }))
            {
                Driver.Run<IAddColumnOperation>(op =>
                                     {
                                         op.TableName = SCHEMA_VERSION_TABLE;
                                         op.Column = c;
                                     });
            }
        }

        /// <summary>
        /// Creates the SchemaInfo table that contains information about applied Migrations.
        /// </summary>
        public void CreateSchemaTable()
        {
            Driver.Run<IAddTableOperation>(
                op =>
                    {
                        op.TableName = "SchemaInfo";
                        op.AddColumn("Version", DbType.Int64, ColumnProperty.NotNull);
                        op.AddColumn(ASSEMBLY_COLUMN);
                        op.AddColumn(TYPE_COLUMN);
                        op.AddColumn(APPLIED_ON_COLUMN);
                    });
        }

        /// <summary>
        /// The highest number applied Migration that has been run against the database.
        /// </summary>
        /// <remarks>
        /// Passing the assembly name allows multiple Migration assemblies to coexist.
        /// </remarks>
        /// <param name="assembly">The name of the assembly that the migrations are from.</param>
        /// <returns>The maximum migration version or zero if none are found.</returns>
        public long CurrentSchemaVersion(string assembly)
        {
            if (!SchemaInfoTableExists())
            {
                return 0;
            }
            using (var reader = Driver.Select(
                op =>
                    {
                        op.TableName = SCHEMA_VERSION_TABLE;
                        op.Columns.Add("MAX(Version)");
                        op.Where(ASSEMBLY_COLUMN.Name, assembly);
                    }))
            {
                if (reader.Read() && ! reader.IsDBNull(0))
                {
                    return reader.GetInt64(0);
                }
                return 0;
            }
        }

        /// <summary>
        /// Get a list of all of the Migration versions that have been applied to the database.
        /// </summary>
        /// <param name="assembly">The name of the assembly that the migrations are from.</param>
        /// <returns>A List of all of the applied migrations or an empty List if none have been applied.</returns>
        public List<long> AppliedMigrations(string assembly)
        {
            if (! SchemaInfoTableExists())
            {
                return new List<long>();
            }

            using (var reader = Driver.Select(
                op =>
                    {
                        op.TableName = SCHEMA_VERSION_TABLE;
                        op.Columns.Add("Version");
                        op.Where(ASSEMBLY_COLUMN.Name, assembly);
                    }))
            {
                var versions = new List<long>();
                while (reader.Read())
                {
                    versions.Add(reader.GetInt64(0));
                }

                versions.Sort();
                return versions;
            }
        }

        /// <summary>
        /// Add a Migration version to the SchemaInfo table. Used when migrating up to a higher version number.
        /// </summary>
        /// <param name="version">The version to add to the SchemaInfo table.</param>
        /// <param name="assembly">The name of the assembly that the migrations are from.</param>
        /// <param name="name">The name of the migration to make it easier for humans to look at the table.</param>
        public void InsertSchemaVersion(long version, string assembly, string name)
        {
            Driver.Run<IInsertOperation>(
                op =>
                    {
                        op.TableName = SCHEMA_VERSION_TABLE;
                        op.Columns.Add("Version");
                        op.Columns.Add(ASSEMBLY_COLUMN.Name);
                        op.Columns.Add(TYPE_COLUMN.Name);
                        op.Columns.Add(APPLIED_ON_COLUMN.Name);
                        op.Values.Add(version);
                        op.Values.Add(assembly);
                        op.Values.Add(name);
                        op.Values.Add(DateTime.Now);
                    });
        }

        /// <summary>
        /// Remove a Migration version from the SchemaInfo table. Used when migrating down to a lower version number.
        /// </summary>
        /// <param name="version">The version to remove.</param>
        /// <param name="assembly">The name of the assembly that the migrations are from.</param>
        public void DeleteSchemaVersion(long version, string assembly)
        {
            Driver.Run<IDeleteOperation>(
                op =>
                    {
                        op.TableName = SCHEMA_VERSION_TABLE;
                        op.Where(Restriction.Equals(ASSEMBLY_COLUMN.Name, assembly), Restriction.Equals("Version", version));
                    });
        }

        private bool SchemaInfoTableExists()
        {
            return Driver.Inspect<ITableExistsOperation>(op => op.TableName = SCHEMA_VERSION_TABLE);
        }
    }
}