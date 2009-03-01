using NUnit.Framework;
using Zorched.Migrations.Framework.Inspection;

namespace Zorched.Migrations.SqlServer.Tests.Execute
{

    [TestFixture]
    public class CreateTableTests : ExecuteBase
    {
        [TearDown]
        public override void Teardown()
        {
            using (var cmd = Connection.CreateCommand())
            {
                cmd.CommandText = "DROP TABLE [dbo].[" + TABLE_NAME + "]";
                cmd.ExecuteNonQuery();
            }
            base.Teardown();
        }

        [Test]
        public void can_add_table()
        {
            CreateTable(TABLE_NAME);

            Assert.IsTrue(Driver.Inspect<ITableExistsOperation>(op => op.TableName = TABLE_NAME));
            Assert.IsTrue(Driver.Inspect<IColumnExistsOperation>(op => {
                                                                         op.TableName = TABLE_NAME;
                                                                         op.ColumnName = "Id";
                                                                     }));
            Assert.IsTrue(Driver.Inspect<IColumnExistsOperation>(op => {
                                                                         op.TableName = TABLE_NAME;
                                                                         op.ColumnName = "Username";
                                                                     }));
        }
    }
}