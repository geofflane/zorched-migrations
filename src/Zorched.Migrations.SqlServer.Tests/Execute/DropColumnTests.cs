using NUnit.Framework;
using Zorched.Migrations.Framework.Inspection;

namespace Zorched.Migrations.SqlServer.Tests.Execute
{

    [TestFixture]
    public class DropColumnTests : ExecuteBase
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
        public void can_drop_column()
        {
            CreateTable(TABLE_NAME);

            Driver.DropColumn(op =>
                                  {
                                      op.TableName = TABLE_NAME;
                                      op.ColumnName = "Email";
                                  });

            Assert.IsTrue(Driver.Inspect<ITableExistsOperation>(op => op.TableName = TABLE_NAME));
            Assert.IsFalse(Driver.Inspect<IColumnExistsOperation>(op => {
                                                                         op.TableName = TABLE_NAME;
                                                                         op.ColumnName = "Email";
                                                                     }));
        }
    }
}