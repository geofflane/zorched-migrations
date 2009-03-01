using NUnit.Framework;
using Zorched.Migrations.SqlServer.Data;

namespace Zorched.Migrations.SqlServer.Tests.Operations
{
    [TestFixture]
    public class UpdateTests
    {
        [Test]
        public void update_with_schema()
        {
            var op = new SqlUpdateOperation { SchemaName = "dbo", TableName = "Foo" };
            op.Columns.Add("var");
            op.Columns.Add("var2");
            Assert.AreEqual("UPDATE [dbo].[Foo] SET [var]=@var,[var2]=@var2", op.ToString());
        }

        [Test]
        public void update_no_schema()
        {
            var op = new SqlUpdateOperation{ SchemaName = null, TableName = "Foo" };
            op.Columns.Add("var");
            op.Columns.Add("var2");
            Assert.AreEqual("UPDATE [Foo] SET [var]=@var,[var2]=@var2", op.ToString());
        }

        [Test]
        public void update_with_where_clause()
        {
            var op = new SqlUpdateOperation { SchemaName = "dbo", TableName = "Foo" };
            op.Columns.Add("var");
            op.Columns.Add("var2");
            op.WhereColumn = "bar";
            op.WhereValue= "baz";

            Assert.AreEqual("UPDATE [dbo].[Foo] SET [var]=@var,[var2]=@var2 WHERE [bar]=@bar", op.ToString());
        }
    }
}