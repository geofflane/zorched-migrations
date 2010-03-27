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
using Zorched.Migrations.Framework;
using Zorched.Migrations.Framework.Schema;
using Zorched.Migrations.Framework.Simple;

namespace Zorched.Migrations.Examples
{
    [Migration(2)]
    public class AddAddressTable
    {
        public const string TABLE_NAME = "addresses";

        public const string FK_COL_NAME = "address_id";
        public const string FK_NAME = "FK_person_address";

        [Up]
        public void AddTableNormalWay(ActionRunner runner)
        {
            runner.AddTable(
                op =>
                {
                    op.TableName = TABLE_NAME;
                    op.AddColumn(new Column { Name = "id", DbType = DbType.Int32, Property = ColumnProperty.PrimaryKeyWithIdentity });
                    op.AddColumn(new Column { Name = "street1", DbType = DbType.String, Size = 50 });
                    op.AddColumn(new Column { Name = "street2", DbType = DbType.String, Size = 50 });
                    op.AddColumn(new Column { Name = "city", DbType = DbType.String, Size = 50 });
                    op.AddColumn(new Column { Name = "state", DbType = DbType.String, Size = 2 });
                    op.AddColumn(new Column { Name = "postalCode", DbType = DbType.String, Size = 12 });
                });

            runner.Run<IAddReferenceAndFkOperation>(op =>
                                                 {
                                                     op.Driver = runner.Driver;
                                                     op.TableName = AddPersonTable.TABLE_NAME;
                                                     op.ColumnName = FK_COL_NAME;
                                                     op.ConstraintName = FK_NAME;
                                                     op.ReferenceTableName = TABLE_NAME;
                                                     op.ReferenceColumnName = "id";
                                                 });
        }

        [Down]
        public void RemoveTable(ActionRunner runner)
        {
            runner.DropConstraint(op =>
                                        {
                                            op.TableName = AddPersonTable.TABLE_NAME;
                                            op.ConstraintName = FK_NAME;
                                        });
            runner.DropColumn(op =>
                                    {
                                        op.TableName = AddPersonTable.TABLE_NAME;
                                        op.ColumnName = FK_COL_NAME;
                                    });
            runner.Run<IDropTableOperation>(op => op.TableName = TABLE_NAME);
        }
    }
}
