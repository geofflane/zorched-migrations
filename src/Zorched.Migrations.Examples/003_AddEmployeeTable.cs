using System.Data;
using Zorched.Migrations.Framework;
using Zorched.Migrations.Framework.Simple;

namespace Zorched.Migrations.Examples
{
    [Migration(3)]
    public class AddEmployeeTable
    {
        [Up]
        public void AddTable(SimpleRunner runner)
        {
            runner.AddTable("dbo", "Employee", new[]
                                                   {
                                                       new Column {Name = "id", DbType = DbType.Int32, Property = ColumnProperty.PrimaryKeyWithIdentity},
                                                       new Column {Name = "name", DbType = DbType.String, Size = 50}
                                                   });
        }

        [Down]
        public void RemoveTable(SimpleRunner runner)
        {
            runner.RemoveTable("dbo", "Employee");
        }
    }
}