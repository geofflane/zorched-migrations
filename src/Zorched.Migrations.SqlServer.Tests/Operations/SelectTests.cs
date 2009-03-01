using NUnit.Framework;
using Zorched.Migrations.SqlServer.Data;

namespace Zorched.Migrations.SqlServer.Tests.Operations
{
    [TestFixture]
    public class SelectTests
    {
        [Test]
        public void select_generates_proper_form_without_columns()
        {
            var op = new SqlSelectOperation {SchemaName = "dbo", TableName = "Foo"};
            Assert.AreEqual("SELECT * FROM [dbo].[Foo]", op.ToString());
        }

        [Test]
        public void select_generates_proper_form_without_columns_no_schema()
        {
            var op = new SqlSelectOperation { SchemaName = null, TableName = "Foo" };
            Assert.AreEqual("SELECT * FROM [Foo]", op.ToString());
        }

        [Test]
        public void select_generates_proper_form_with_columns()
        {
            var op = new SqlSelectOperation { SchemaName = "dbo", TableName = "Foo" };
            op.Columns.Add("Id");
            op.Columns.Add("Bar");
            Assert.AreEqual("SELECT Id,Bar FROM [dbo].[Foo]", op.ToString());
        }

        [Test]
        public void select_generates_proper_form_with_columns_and_where_column()
        {
            var op = new SqlSelectOperation { SchemaName = "dbo", TableName = "Foo" };
            op.Columns.Add("Id");
            op.Columns.Add("Bar");
            op.WhereColumn = "Baz";
            op.WhereValue = "123";

            Assert.AreEqual("SELECT Id,Bar FROM [dbo].[Foo] WHERE [Baz]=@Baz", op.ToString());
        }

        [Test]
        public void select_generates_proper_form_with_columns_and_where_clause()
        {
            var op = new SqlSelectOperation { SchemaName = "dbo", TableName = "Foo" };
            op.Columns.Add("Id");
            op.Columns.Add("Bar");
            op.WhereClause = "Baz=123";

            Assert.AreEqual("SELECT Id,Bar FROM [dbo].[Foo] WHERE  Baz=123", op.ToString());
        }
    }
}