using NUnit.Framework;
using Zorched.Migrations.Framework.Data;

namespace Zorched.Migrations.SqlServer.Tests.Execute
{
    [TestFixture]
    public class DeleteDataTests : ExecuteBase
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
        public void can_delete_data()
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

            int id;
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

            Driver.Delete(op =>
                              {
                                  op.TableName = TABLE_NAME;
                                  op.Where("Id", id);
                              });

            using (var reader = Driver.Read<IGenericReaderOperation>(op =>
                                                                  {
                                                                      op.Sql = "SELECT * FROM [" + TABLE_NAME +
                                                                               "] WHERE id=" + id;
                                                                  }))
            {
                Assert.IsFalse(reader.Read());
            }
        }
    }
}