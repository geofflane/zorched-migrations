using NUnit.Framework;
using Zorched.Migrations.SqlServer.Data;

namespace Zorched.Migrations.SqlServer.Tests
{

    [TestFixture]
    public class InsertTests
    {
        [Test]
        public void insert_with_schema()
        {
            var op = new SqlInsertOperation() { SchemaName = "dbo", TableName = "Foo" };
            op.Columns.Add("var");
            op.Columns.Add("var2");
            Assert.AreEqual("INSERT INTO [dbo].[Foo] ([var],[var2]) VALUES (@var,@var2)", op.ToString());
        }

        [Test]
        public void insert_no_schema()
        {
            var op = new SqlInsertOperation { SchemaName = null, TableName = "Foo" };
            op.Columns.Add("var");
            op.Columns.Add("var2");
            Assert.AreEqual("INSERT INTO [Foo] ([var],[var2]) VALUES (@var,@var2)", op.ToString());
        }
    }
}
