using NUnit.Framework;
using Zorched.Migrations.Framework.Inspection;
using Zorched.Migrations.Framework.Schema;

namespace Zorched.Migrations.SqlServer.Tests.Execute
{
    [TestFixture]
    public class RenameColumnTests : ExecuteBase
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
        public void can_rename_column()
        {
            CreateTable(TABLE_NAME);

            Driver.Run<IRenameColumnOperation>(op =>
                                                   {
                                                       op.TableName = TABLE_NAME;
                                                       op.ColumnName = "Email";
                                                       op.NewColumnName = "EmailAddress";
                                                   });

            Assert.IsTrue(Driver.Inspect<ITableExistsOperation>(op => op.TableName = TABLE_NAME));
            Assert.IsFalse(Driver.Inspect<IColumnExistsOperation>(op =>
                                                                      {
                                                                          op.TableName = TABLE_NAME;
                                                                          op.ColumnName = "Email";
                                                                      }));
            Assert.IsTrue(Driver.Inspect<IColumnExistsOperation>(op =>
                                                                     {
                                                                         op.TableName = TABLE_NAME;
                                                                         op.ColumnName = "EmailAddress";
                                                                     }));
        }
    }
}