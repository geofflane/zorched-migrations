using NUnit.Framework;
using Zorched.Migrations.Framework.Data;

namespace Zorched.Migrations.SqlServer.Tests.Execute
{
    [TestFixture]
    public class UpdateDataTests : ExecuteBase
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
        public void can_update_data()
        {
            CreateTable(TABLE_NAME);

            Driver.Insert(op =>
                              {
                                  op.TableName = TABLE_NAME;
                                  op.Columns.Add("Username");
                                  op.Columns.Add("Password");
                                  op.Values.Add("TestUser");
                                  op.Values.Add("TestPass");
                              });

            int id = 0;

            using (var reader = Driver.Read<ISelectOperation>(op =>
                                                                  {
                                                                      op.TableName = TABLE_NAME;
                                                                      op.Columns.Add("Id");
                                                                      op.Columns.Add("Username");
                                                                      op.Columns.Add("Password");
                                                                  }))
            {
                Assert.IsTrue(reader.Read());
                id = reader.GetInt32(0);
                Assert.AreEqual("TestUser", reader.GetString(1));
                Assert.AreEqual("TestPass", reader.GetString(2));
            }

            Driver.Update(op =>
                              {
                                  op.TableName = TABLE_NAME;
                                  op.Columns.Add("Username");
                                  op.Values.Add("Updated");
                                  op.Where("Id", id);
                              });


            using (var reader = Driver.Read<ISelectOperation>(op =>
                                                                  {
                                                                      op.TableName = TABLE_NAME;
                                                                      op.Columns.Add("Id");
                                                                      op.Columns.Add("Username");
                                                                      op.Columns.Add("Password");
                                                                  }))
            {
                Assert.IsTrue(reader.Read());
                Assert.AreEqual("Updated", reader.GetString(1));
                Assert.AreEqual("TestPass", reader.GetString(2));
            }
        }
    }
}