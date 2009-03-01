using System.Data;
using NUnit.Framework;
using Zorched.Migrations.Framework;
using Zorched.Migrations.Framework.Inspection;

namespace Zorched.Migrations.SqlServer.Tests.Execute
{

    [TestFixture]
    public class AddColumnTests : ExecuteBase
    {
        [TearDown]
        public override void Teardown()
        {
            using (var cmd = Database.CreateCommand())
            {
                cmd.CommandText = "DROP TABLE [dbo].[" + TABLE_NAME + "]";
                cmd.ExecuteNonQuery();
            }
            base.Teardown();
        }

        [Test]
        public void can_add_column()
        {
            CreateTable(TABLE_NAME);

            Driver.AddColumn(op =>
                                 {
                                     op.TableName = TABLE_NAME;
                                     op.Column = new Column {Name = "Newcolumn", DbType = DbType.String};
                                 });

            Assert.IsTrue(Driver.Inspect<ITableExistsOperation>(op => op.TableName = TABLE_NAME));
            Assert.IsTrue(Driver.Inspect<IColumnExistsOperation>(op => {
                                                                         op.TableName = TABLE_NAME;
                                                                         op.ColumnName = "Newcolumn";
                                                                     }));
        }
    }
}