using System;
using NUnit.Framework;
using Zorched.Migrations.SqlServer.Schema;

namespace Zorched.Migrations.SqlServer.Tests
{

    [TestFixture]
    public class DropConstraintTests
    {
        
        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void drop_constraint_throws_exception_without_tablename()
        {
            var op = new SqlDropConstraintOperation { SchemaName = "dbo", TableName = null, ConstraintName = "FK_Foo"};
            op.ToString();
            Assert.Fail();
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void drop_constraint_throws_exception_without_constraint()
        {
            var op = new SqlDropConstraintOperation { SchemaName = "dbo", TableName = "Foo", ConstraintName = null };
            op.ToString();
            Assert.Fail();
        }

        [Test]
        public void drop_constraint_creates_expected_sql_with_schema_and_table()
        {
            var op = new SqlDropConstraintOperation { SchemaName = "dbo", TableName = "Foo", ConstraintName = "FK_Foo"};
            Assert.AreEqual("ALTER TABLE [dbo].[Foo] DROP CONSTRAINT [FK_Foo]", op.ToString());
        }

        [Test]
        public void drop_table_creates_expected_sql_with_just_table()
        {
            var op = new SqlDropConstraintOperation { SchemaName = null, TableName = "Foo", ConstraintName = "FK_Foo" };
            Assert.AreEqual("ALTER TABLE [Foo] DROP CONSTRAINT [FK_Foo]", op.ToString());
        }
    }
}