using System.Data;
using Zorched.Migrations.Framework;
using Zorched.Migrations.Framework.Fluent;
using Zorched.Migrations.Framework.Schema;

namespace Zorched.Migrations.Examples
{
    [Migration(1)]
    public class AddPersonTable
    {
        public const string TABLE_NAME = "people";

        [Up]
        public void AddFluentTable(IDriver driver)
        {
            var database = new FluentDriver(driver);
            database.AddTable
                .UsingSchema("dbo").WithName(TABLE_NAME)
                .WithColumn.Named("id").OfType(DbType.Int32).That.IsPrimaryKey.Identity
                .WithColumn.Named("first_name").OfType(DbType.String)
                .WithColumn.Named("middle_name").OfType(DbType.String)
                .WithColumn.Named("last_name").OfType(DbType.String)
                .Add();
        }

        [Down]
        public void RemoveTable(IDriver database)
        {
            database.Run<IDropTableOperation>(op => op.TableName = TABLE_NAME);
        }
    }
}