// Copyright 2008, 2009 Geoff Lane <geoff@zorched.net>
// 
// This file is part of Zorched Migrations a SQL Schema Migration framework for .NET.
// 
// Zorched Migrations is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Zorched Migrations is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

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
            using (var cmd = Database.CreateCommand())
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