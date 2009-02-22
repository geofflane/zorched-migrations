using System.Data;
using Zorched.Migrations.Framework;
using Zorched.Migrations.Framework.Schema;

namespace Zorched.Migrations.Examples
{
    [Migration(1)]
    public class AddNewTable
    {
        private const string TABLE_NAME = "Test";

        public interface ICustomOp : IOperation
        {
            string Variable { get; set; }
        }

        public class CustomOp : ICustomOp
        {
            public string Variable { get; set; }
            public void Execute(IDbCommand command)
            {
                // ...
            }

            public override string ToString()
            {
                return "some kind of sql";
            }
        }


        [Setup]
        public void Setup(IOperationRepository driver)
        {
            driver.Register<ICustomOp>(typeof(CustomOp));
        }

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

            database.Run<ICustomOp>(
                op =>
                    {
                        op.Variable = "Foo";   
                    });

            database.Run((ICustomOp op) => op.Variable = "Foo" );
        }

        [Up]
        [OnlyWhenDriver("SqlServer;Sqlite")]  // Only run when SqlServer or Sqlite Driver is running this migration
        public void AddTable2(IDriver database)
        {
            database.AddTable(
                op =>
                {
                    op.TableName = TABLE_NAME+"2";
                    op.AddColumn(new Column { Name = "Id", DbType = DbType.Int32 });
                    op.AddColumn(new Column { Name = "Title", DbType = DbType.String, Size = 50 });
                });
        }

        [Down]
        public void RemoveTable(IDriver database)
        {
            database.Run<IDropTableOperation>(
                op => op.TableName = TABLE_NAME
                );
        }
    }
}