using NUnit.Framework;
using Zorched.Migrations.SqlServer.Schema;

namespace Zorched.Migrations.SqlServer.Tests.Operations
{
    [TestFixture]
    public class RenameColumnTests
    {

        [Test]
        public void rename_column_creates_expected_sql_with_schema_and_table()
        {
            var op = new SqlRenameColumnOperation { SchemaName = "dbo", TableName = "Foo", ColumnName = "Bar", NewColumnName = "New"};
            Assert.AreEqual("EXEC sp_rename 'dbo.Foo.Bar', 'New', 'COLUMN'", op.ToString());
        }

        [Test]
        public void rename_column_creates_expected_sql_with_just_table()
        {
            var op = new SqlRenameColumnOperation { SchemaName = null, TableName = "Foo", ColumnName = "Bar", NewColumnName = "New" };
            Assert.AreEqual("EXEC sp_rename 'Foo.Bar', 'New', 'COLUMN'", op.ToString());
        }
    }
}