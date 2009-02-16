using System;
using NUnit.Framework;
using Zorched.Migrations.SqlServer.Schema;

namespace Zorched.Migrations.SqlServer.Tests
{

    [TestFixture]
    public class DropColumnTests
    {
        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void drop_table_throws_exception_without_tablename()
        {
            var op = new SqlDropColumnOperation { SchemaName = "dbo", TableName = null, ColumnName = "Foo"};
            op.CreateSql();
            Assert.Fail();
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void drop_table_throws_exception_without_columnname()
        {
            var op = new SqlDropColumnOperation { SchemaName = "dbo", TableName = "Foo", ColumnName = null };
            op.CreateSql();
            Assert.Fail();
        }

        [Test]
        public void drop_table_creates_expected_sql_with_schema_and_table()
        {
            var op = new SqlDropColumnOperation {SchemaName = "dbo", TableName = "Foo", ColumnName = "Bar"};
            Assert.AreEqual("ALTER TABLE [dbo].[Foo] DROP COLUMN [Bar]", op.CreateSql());
        }

        [Test]
        public void drop_table_creates_expected_sql_with_just_table()
        {
            var op = new SqlDropColumnOperation { SchemaName = null, TableName = "Foo", ColumnName = "Bar" };
            Assert.AreEqual("ALTER TABLE [Foo] DROP COLUMN [Bar]", op.CreateSql());
        }
    }
}
