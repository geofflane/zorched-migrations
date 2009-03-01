using System.Data;
using NUnit.Framework;
using Zorched.Migrations.Framework;
using Zorched.Migrations.Framework.Inspection;
using Zorched.Migrations.Framework.Schema;

namespace Zorched.Migrations.SqlServer.Tests.Execute
{
    [TestFixture]
    public class AddConstraintsTests : ExecuteBase
    {
        private const string TABLE_NAME2 = "OtherUser";

        [TearDown]
        public override void Teardown()
        {
            using (var cmd = Connection.CreateCommand())
            {
                cmd.CommandText = "DROP TABLE [dbo].[" + TABLE_NAME + "]";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "DROP TABLE [dbo].[" + TABLE_NAME2 + "]";
                cmd.ExecuteNonQuery();
            }
            base.Teardown();
        }

        [Test]
        public void can_add_unique_constraint()
        {
            CreateTable(TABLE_NAME);
            CreateTable(TABLE_NAME2);

            Driver.Run<IAddUniqueConstraintOperation>(op =>
                                                          {
                                                              op.ConstraintName = "UN_User_Email";
                                                              op.TableName = TABLE_NAME;
                                                              op.ColumnName = "Email";
                                                          });

            Assert.IsTrue(Driver.Inspect<ITableExistsOperation>(op => op.TableName = TABLE_NAME));
            Assert.IsTrue(Driver.Inspect<IColumnExistsOperation>(op =>
                                                                     {
                                                                         op.TableName = TABLE_NAME;
                                                                         op.ColumnName = "Email";
                                                                     }));

            Driver.DropConstraint(op =>
                                      {
                                          op.TableName = TABLE_NAME;
                                          op.ConstraintName = "UN_User_Email";
                                      });
        }

        [Test]
        public void can_add_check_constraint()
        {
            CreateTable(TABLE_NAME);
            CreateTable(TABLE_NAME2);

            Driver.Run<IAddCheckConstraintOperation>(op =>
                                                         {
                                                             op.ConstraintName = "CK_User_Email";
                                                             op.TableName = TABLE_NAME;
                                                             op.ConstraintDefinition = "Email <> 'Foo'";
                                                         });

            Assert.IsTrue(Driver.Inspect<ITableExistsOperation>(op => op.TableName = TABLE_NAME));
            Assert.IsTrue(Driver.Inspect<IColumnExistsOperation>(op =>
                                                                     {
                                                                         op.TableName = TABLE_NAME;
                                                                         op.ColumnName = "Email";
                                                                     }));
            Driver.DropConstraint(op =>
                                      {
                                          op.TableName = TABLE_NAME;
                                          op.ConstraintName = "CK_User_Email";
                                      });
        }

        [Test]
        public void can_add_foreign_key_constraint()
        {
            CreateTable(TABLE_NAME);
            CreateTable(TABLE_NAME2);

            Driver.AddColumn(op =>
                                 {
                                     op.TableName = TABLE_NAME;
                                     op.Column = new Column {Name = "OtherUserId", DbType = DbType.Int32};
                                 });

            Driver.Run<IAddForeignKeyOperation>(op =>
                                                    {
                                                        op.ConstraintName = "FK_User_User2";
                                                        op.TableName = TABLE_NAME;
                                                        op.ColumnName = "OtherUserId";
                                                        op.ReferenceTableName = TABLE_NAME2;
                                                        op.ReferenceColumnName = "Id";
                                                    });

            Assert.IsTrue(Driver.Inspect<ITableExistsOperation>(op => op.TableName = TABLE_NAME));
            Assert.IsTrue(Driver.Inspect<IColumnExistsOperation>(op =>
                                                                     {
                                                                         op.TableName = TABLE_NAME;
                                                                         op.ColumnName = "Email";
                                                                     }));

            Driver.DropConstraint(op =>
                                      {
                                          op.TableName = TABLE_NAME;
                                          op.ConstraintName = "FK_User_User2";
                                      });
        }
    }
}