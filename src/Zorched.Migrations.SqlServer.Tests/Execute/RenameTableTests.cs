using NUnit.Framework;
using Zorched.Migrations.Framework.Inspection;
using Zorched.Migrations.Framework.Schema;

namespace Zorched.Migrations.SqlServer.Tests.Execute
{

    [TestFixture]
    public class RenameTableTests : ExecuteBase
    {
        private const string NEW_TABLE = "MyUserTable";

        [TearDown]
        public override void Teardown()
        {
            using (var cmd = Database.CreateCommand())
            {
                cmd.CommandText = "DROP TABLE [dbo].[" + NEW_TABLE + "]";
                cmd.ExecuteNonQuery();
            }
            base.Teardown();
        }

        [Test]
        public void can_rename_table()
        {
            CreateTable(TABLE_NAME);

            Driver.Run<IRenameTableOperation>(op =>
                                                  {
                                                      op.TableName = TABLE_NAME;
                                                      op.NewTableName = NEW_TABLE;
                                                  });

            Assert.IsFalse(Driver.Inspect<ITableExistsOperation>(op => op.TableName = TABLE_NAME));
            Assert.IsTrue(Driver.Inspect<ITableExistsOperation>(op => op.TableName = NEW_TABLE));
        }
    }
}