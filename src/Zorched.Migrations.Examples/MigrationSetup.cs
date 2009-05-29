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

using System;
using System.Data;
using Zorched.Migrations.Framework;
using Zorched.Migrations.Framework.Schema;

namespace Zorched.Migrations.Examples
{
    public interface IAddReferenceAndFkOperation : IAddForeignKeyOperation
    {
        IDriver Driver { get; set; }
    }

    public class AddRefAndFKOp : IAddReferenceAndFkOperation
    {
        public string SchemaName { get; set; }
        public string TableName { get; set; }
        public string ConstraintName { get; set; }
        public string ColumnName { get; set; }
        public ConstraintProperty Property { get; set; }
        public string ReferenceSchemaName { get; set; }
        public string ReferenceTableName { get; set; }
        public string ReferenceColumnName { get; set; }

        public IDriver Driver { get; set; }

        public void Execute(IDbCommand command)
        {
            Driver.Run<IAddColumnOperation>(op =>
                                                {
                                                    op.SchemaName = SchemaName;
                                                    op.TableName = TableName;
                                                    op.Column = new Column {Name = ColumnName, DbType = DbType.Int32};
                                                });

            Driver.Run<IAddForeignKeyOperation>(op =>
                                                    {
                                                        op.ConstraintName = ConstraintName;
                                                        op.SchemaName = SchemaName;
                                                        op.TableName = TableName;
                                                        op.ColumnName = ColumnName;
                                                        op.ReferenceSchemaName = ReferenceSchemaName;
                                                        op.ReferenceTableName = ReferenceTableName;
                                                        op.ReferenceColumnName = ReferenceColumnName;
                                                    });
        }
    }

    [Setup]
    public class MigrationSetup
    {
        [Setup]
        public void SetupFk(IOperationRepository repos)
        {
            Console.Out.WriteLine("XXX: In Setup");
            repos.Register<IAddReferenceAndFkOperation>(typeof (AddRefAndFKOp));
        }
    }
}