using NUnit.Framework;
using Zorched.Migrations.SqlServer.Schema;

namespace Zorched.Migrations.SqlServer.Tests.Operations
{
    [TestFixture]
    public class RenameTableTests
    {
        [Test]
        public void rename_table_creates_expected_sql_with_schema_and_table()
        {
            var op = new SqlRenameTableOperation { SchemaName = "dbo", TableName = "Foo", NewTableName = "Bar"};
            Assert.AreEqual("EXEC sp_rename 'dbo.Foo', 'Bar'", op.ToString());
        }

        [Test]
        public void rename_table_creates_expected_sql_with_just_table()
        {
            var op = new SqlRenameTableOperation { SchemaName = null, TableName = "Foo", NewTableName = "Bar" };
            Assert.AreEqual("EXEC sp_rename 'Foo', 'Bar'", op.ToString());
        }
    }
}