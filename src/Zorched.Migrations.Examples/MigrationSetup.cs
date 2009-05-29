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