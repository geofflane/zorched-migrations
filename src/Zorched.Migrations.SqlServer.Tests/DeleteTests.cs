using NUnit.Framework;
using Zorched.Migrations.SqlServer.Data;

namespace Zorched.Migrations.SqlServer.Tests
{

    [TestFixture]
    public class DeleteTests
    {
        
        [Test]
        public void delete_with_schema()
        {
            var op = new SqlDeleteOperation {SchemaName = "dbo", TableName = "Foo"};
            Assert.AreEqual("DELETE FROM [dbo].[Foo]", op.ToString());
        }

        [Test]
        public void delete_no_schema()
        {
            var op = new SqlDeleteOperation { SchemaName = null, TableName = "Foo" };
            Assert.AreEqual("DELETE FROM [Foo]", op.ToString());
        }

        [Test]
        public void delete_with_wherecolumn()
        {
            var op = new SqlDeleteOperation { SchemaName = "dbo", TableName = "Foo", WhereColumn = "Bar", WhereValue = 123 };
            Assert.AreEqual("DELETE FROM [dbo].[Foo] WHERE [Bar]=@Bar", op.ToString());
        }

        [Test]
        public void delete_with_whereclause()
        {
            var op = new SqlDeleteOperation { SchemaName = "dbo", TableName = "Foo", WhereClause = "Bar=123" };
            Assert.AreEqual("DELETE FROM [dbo].[Foo] WHERE  Bar=123", op.ToString());
        }
    }
}