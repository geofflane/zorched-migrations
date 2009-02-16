using System;
using NUnit.Framework;
using Zorched.Migrations.SqlServer.Schema;

namespace Zorched.Migrations.SqlServer.Tests
{

    [TestFixture]
    public class DropTableTests
    {

        [Test]
        public void drop_table_creates_expected_sql_with_schema_and_table()
        {
            var op = new SqlDropTableOperation {SchemaName = "dbo", TableName = "Foo"};
            Assert.AreEqual("DROP TABLE [dbo].[Foo]", op.CreateSql());
        }

        [Test]
        public void drop_table_creates_expected_sql_with_just_table()
        {
            var op = new SqlDropTableOperation { SchemaName = null, TableName = "Foo" };
            Assert.AreEqual("DROP TABLE [Foo]", op.CreateSql());
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void drop_table_throws_exception_without_tablename()
        {
            var op = new SqlDropTableOperation { SchemaName = "dbo", TableName = null };
            op.CreateSql();
            Assert.Fail();
        }
    }
}