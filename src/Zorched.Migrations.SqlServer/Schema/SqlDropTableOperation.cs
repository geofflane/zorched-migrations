using System;
using System.Text;
using Zorched.Migrations.Framework.Schema;

namespace Zorched.Migrations.SqlServer.Schema
{
    public class SqlDropTableOperation : BaseSchemaOperation, IDropTableOperation
    {
        public SqlDropTableOperation()
        {
            SchemaName = "dbo";
        }

        public override string CreateSql()
        {
            if (string.IsNullOrEmpty(TableName))
                throw new ArgumentException("TableName must be set.");

            var sb = new StringBuilder("DROP TABLE ");
            AddTableInfo(sb, SchemaName, TableName);
            return sb.ToString();
        }
    }
}