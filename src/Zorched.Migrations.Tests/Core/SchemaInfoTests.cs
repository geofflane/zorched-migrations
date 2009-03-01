using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using NUnit.Framework;
using Zorched.Migrations.Core;
using Zorched.Migrations.Framework.Data;
using Zorched.Migrations.Framework.Inspection;
using Zorched.Migrations.SqlServer;
using Zorched.Migrations.Tests.Framework;

namespace Zorched.Migrations.Tests.Core
{

    [TestFixture]
    public class SchemaInfoTests
    {

        protected SqlServerDriver Driver;
        protected DbParams Database;

        [SetUp]
        public virtual void Setup()
        {
            var cs = ConfigurationManager.ConnectionStrings["SqlServer"];
            var factory = DbProviderFactories.GetFactory(cs.ProviderName);
            var connection = factory.CreateConnection();
            connection.ConnectionString = cs.ConnectionString;

            Database = new DbParams(connection);
            Database.BeginTransaction();

            Driver = new SqlServerDriver(Database, new TestLogger());
        }

        [TearDown]
        public virtual void Teardown()
        {
            Database.Transaction.Rollback();
            Database.Connection.Close();
        }

        [Test]
        public void can_ensure_schema_info()
        {
            var si = new SchemaInfo(Driver);
            si.EnsureSchemaTable();

            Assert.IsTrue(Driver.Inspect<ITableExistsOperation>(op => op.TableName = "SchemaInfo"));
        }

        [Test]
        public void can_insert_schema_version()
        {
            var si = new SchemaInfo(Driver);
            si.EnsureSchemaTable();

            si.InsertSchemaVersion(10);

            using (var reader = Driver.Read<IGenericReaderOperation>(op => op.Sql = "SELECT * FROM SchemaInfo WHERE Version=10"))
            {
                Assert.IsTrue(reader.Read());
            }
        }

        [Test]
        public void can_remove_schema_version()
        {
            can_insert_schema_version();

            var si = new SchemaInfo(Driver);
            si.DeleteSchemaVersion(10);

            using (var reader = Driver.Read<IGenericReaderOperation>(op => op.Sql = "SELECT * FROM SchemaInfo WHERE Version=10"))
            {
                Assert.IsFalse(reader.Read());
            }
        }

        [Test]
        public void can_get_most_recent_version()
        {
            var si = new SchemaInfo(Driver);
            si.EnsureSchemaTable();

            si.InsertSchemaVersion(10);
            si.InsertSchemaVersion(100);
            si.InsertSchemaVersion(1);

            Assert.AreEqual(100, si.CurrentSchemaVersion());
        }

        [Test]
        public void can_get_all_version()
        {
            var si = new SchemaInfo(Driver);
            si.EnsureSchemaTable();

            si.InsertSchemaVersion(10);
            si.InsertSchemaVersion(100);
            si.InsertSchemaVersion(1);

            IList<long> versions = si.AppliedMigrations();
            Assert.AreEqual(3, versions.Count);
            Assert.AreEqual(1, versions[0]);
            Assert.AreEqual(100, versions[2]);
        }
    }
}