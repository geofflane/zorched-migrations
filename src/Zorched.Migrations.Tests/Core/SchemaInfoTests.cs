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

            si.InsertSchemaVersion(10, "assembly", "foo");

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
            si.DeleteSchemaVersion(10, "assembly");

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

            si.InsertSchemaVersion(10, "assembly", "foo");
            si.InsertSchemaVersion(100, "assembly", "bar");
            si.InsertSchemaVersion(1, "assembly", "baz");

            Assert.AreEqual(100, si.CurrentSchemaVersion("assembly"));
        }

        [Test]
        public void can_get_all_version()
        {
            var si = new SchemaInfo(Driver);
            si.EnsureSchemaTable();

            si.InsertSchemaVersion(10, "assembly", "foo");
            si.InsertSchemaVersion(100, "assembly", "bar");
            si.InsertSchemaVersion(1, "assembly", "baz");

            IList<long> versions = si.AppliedMigrations("assembly");
            Assert.AreEqual(3, versions.Count);
            Assert.AreEqual(1, versions[0]);
            Assert.AreEqual(100, versions[2]);
        }
    }
}