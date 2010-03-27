using System;
using Zorched.Migrations.Framework.Schema;

namespace Zorched.Migrations.Framework.Simple
{
    public class SimpleRunner : Runner
    {
        public SimpleRunner(IDriver driver) : base(driver)
        {
        }

        public void AddTable(string schema, string tableName, Column[] columns)
        {
            Driver.Run<IAddTableOperation>(op =>
                                               {
                                                   op.SchemaName = schema;
                                                   op.TableName = "Employee";
                                                   Array.ForEach(columns, op.AddColumn);
                                               });
        }

        public void RemoveTable(string schema, string tableName)
        {
            Driver.Run<IDropTableOperation>(op =>
                                                {
                                                    op.SchemaName = schema;
                                                    op.TableName = tableName;
                                                });
        }
    }
}