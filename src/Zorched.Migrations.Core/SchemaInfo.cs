using System.Data;
using Zorched.Migrations.Framework;

namespace Zorched.Migrations.Core
{
    public class SchemaInfo
    {
        public const string SCHEMA_VERSION_TABLE = "SchemaInfo";

        public SchemaInfo(IDriver driver)
        {
            Driver = driver;
        }

        public IDriver Driver { get; set; }

        public void CreateSchemaTable()
        {
            Driver.AddTable(
                op =>
                {
                    op.TableName = "SchemaInfo";
                    op.AddColumn("Version", DbType.UInt64, ColumnProperty.NotNull);
                    op.AddColumn("AppliedOn", DbType.DateTime, ColumnProperty.NotNull, "(getdate())");
                });
        }

        public long LastMigration()
        {
            var reader = Driver.Select(
                op =>
                {
                    op.TableName = SCHEMA_VERSION_TABLE;
                    op.Columns.Add("MAX(VERSION)");
                });

            return reader.Read() ? reader.GetInt64(0) : 0;
        }

        public void InsertSchemaVersion(long version)
        {
            Driver.Insert(
                op =>
                {
                    op.TableName = SCHEMA_VERSION_TABLE;
                    op.Columns.Add("Version");
                    op.Values.Add(version);
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