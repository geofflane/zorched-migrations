using System;
using System.Text;
using Zorched.Migrations.Framework.Schema;

namespace Zorched.Migrations.SqlServer.Schema
{
    public class SqlDropColumnOperation : BaseSchemaOperation, IDropColumnOperation
    {
        public string ColumnName { get; set; }

        public override string CreateSql()
        {
            if (string.IsNullOrEmpty(TableName))
                throw new ArgumentException("TableName must be set.");

            if (string.IsNullOrEmpty(ColumnName))
                throw new ArgumentException("ColumnName must be set.");

            var sb = new StringBuilder("ALTER TABLE ");
            AddTableInfo(sb, SchemaName, TableName);
            sb.Append(" DROP COLUMN ").AppendFormat(QUOTE_FORMAT, ColumnName);

            return sb.ToString();
        }
    }
}