using System.Data;
using Zorched.Migrations.Framework;

namespace Zorched.Migrations.Examples
{
    [Migration(1)]
    public class AddNewTable
    {
        private const string TABLE_NAME = "Test";

        [Up]
        public void AddTable(IDriver database)
        {
            database.AddTable(
                op =>
                    {
                        op.TableName = TABLE_NAME;
                        op.AddColumn(new Column {Name = "Id", DbType = DbType.Int32});
                        op.AddColumn(new Column {Name = "Title", DbType = DbType.String, Size = 50});
                    });
        }

        [Down]
        public void RemoveTable(IDriver database)
        {
            database.Drop(
                op => op.TableName = TABLE_NAME
                );
        }
    }
}