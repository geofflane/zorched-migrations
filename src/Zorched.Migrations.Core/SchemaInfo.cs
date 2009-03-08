using System;
using System.Collections.Generic;
using System.Data;
using Zorched.Migrations.Framework;
using Zorched.Migrations.Framework.Inspection;

namespace Zorched.Migrations.Core
{
    public class SchemaInfo : ISchemaInfo
    {
        public const string SCHEMA_VERSION_TABLE = "SchemaInfo";

        public SchemaInfo(IDriver driver)
        {
            Driver = driver;
        }

        public IDriver Driver { get; set; }

        public void EnsureSchemaTable()
        {
            if (! Driver.Inspect<ITableExistsOperation>(op => op.TableName = SCHEMA_VERSION_TABLE))
            {
                CreateSchemaTable();
            }
        }

        public void CreateSchemaTable()
        {
            Driver.AddTable(
                op =>
                {
                    op.TableName = "SchemaInfo";
                    op.AddColumn("Version", DbType.Int64, ColumnProperty.NotNull);
                    op.AddColumn("Type", DbType.String, 255, ColumnProperty.NotNull);
                    op.AddColumn("AppliedOn", DbType.DateTime, ColumnProperty.NotNull, "(getdate())");
                });
        }

        public long CurrentSchemaVersion()
        {
            using (var reader = Driver.Select(
                op =>
                {
                    op.TableName = SCHEMA_VERSION_TABLE;
                    op.Columns.Add("MAX(Version)");
                }))
            {
                return reader.Read() ? reader.GetInt64(0) : 0;
            }

        }

        public List<long> AppliedMigrations()
        {
            using (var reader = Driver.Select(
                op =>
                {
                    op.TableName = SCHEMA_VERSION_TABLE;
                    op.Columns.Add("Version");
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


        public void InsertSchemaVersion(long version, string name)
        {
            Driver.Insert(
                op =>
                {
                    op.TableName = SCHEMA_VERSION_TABLE;
                    op.Columns.Add("Version");
                    op.Columns.Add("Type");
                    op.Columns.Add("AppliedOn");
                    op.Values.Add(version);
                    op.Values.Add(name);
                    op.Values.Add(DateTime.Now);
                });
        }

        public void DeleteSchemaVersion(long version)
        {
            Driver.Delete(
                op =>
                {
                    op.TableName = SCHEMA_VERSION_TABLE;
                    op.WhereColumn = "Version";
                    op.WhereValue = version;
                });
        }
    }
}