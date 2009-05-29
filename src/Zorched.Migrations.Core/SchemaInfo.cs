using System;
using System.Collections.Generic;
using System.Data;
using Zorched.Migrations.Framework;
using Zorched.Migrations.Framework.Inspection;

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

        public void EnsureColumn(Column c)
        {
            if (!Driver.Inspect<IColumnExistsOperation>(op =>
                                                            {
                                                                op.TableName = SCHEMA_VERSION_TABLE;
                                                                op.ColumnName = c.Name;
                                                            }))
            {
                Driver.AddColumn(op =>
                                     {
                                         op.TableName = SCHEMA_VERSION_TABLE;
                                         op.Column = c;
                                     });
            }
        }

        public void CreateSchemaTable()
        {
            Driver.AddTable(
                op =>
                    {
                        op.TableName = "SchemaInfo";
                        op.AddColumn("Version", DbType.Int64, ColumnProperty.NotNull);
                        op.AddColumn(ASSEMBLY_COLUMN);
                        op.AddColumn(TYPE_COLUMN);
                        op.AddColumn(APPLIED_ON_COLUMN);
                    });
        }

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


        public void InsertSchemaVersion(long version, string assembly, string name)
        {
            Driver.Insert(
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

        public void DeleteSchemaVersion(long version, string assembly)
        {
            Driver.Delete(
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