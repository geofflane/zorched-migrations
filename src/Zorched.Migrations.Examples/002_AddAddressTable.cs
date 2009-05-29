using System.Data;
using Zorched.Migrations.Framework;
using Zorched.Migrations.Framework.Schema;

namespace Zorched.Migrations.Examples
{
    [Migration(2)]
    public class AddAddressTable
    {
        public const string TABLE_NAME = "addresses";

        public const string FK_COL_NAME = "address_id";
        public const string FK_NAME = "FK_person_address";

        [Up]
        public void AddTableNormalWay(IDriver database)
        {
            database.AddTable(
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

            database.Run<IAddReferenceAndFkOperation>(op =>
                                                 {
                                                     op.Driver = database;
                                                     op.TableName = AddPersonTable.TABLE_NAME;
                                                     op.ColumnName = FK_COL_NAME;
                                                     op.ConstraintName = FK_NAME;
                                                     op.ReferenceTableName = TABLE_NAME;
                                                     op.ReferenceColumnName = "id";
                                                 });
        }

        [Down]
        public void RemoveTable(IDriver database)
        {
            database.DropConstraint(op =>
                                        {
                                            op.TableName = AddPersonTable.TABLE_NAME;
                                            op.ConstraintName = FK_NAME;
                                        });
            database.DropColumn(op =>
                                    {
                                        op.TableName = AddPersonTable.TABLE_NAME;
                                        op.ColumnName = FK_COL_NAME;
                                    });
            database.Run<IDropTableOperation>(op => op.TableName = TABLE_NAME);
        }
    }
}
