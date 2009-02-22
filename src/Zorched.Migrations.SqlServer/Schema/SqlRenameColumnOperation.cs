using System.Text;
using Zorched.Migrations.Framework.Schema;

namespace Zorched.Migrations.SqlServer.Schema
{
    public class SqlRenameColumnOperation : BaseSchemaOperation, IRenameColumnOperation
    {
        private const string QUOTE_VALUE = "'{0}'";

        public string ColumnName { get; set; }

        public string NewColumnName { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder("EXEC sp_rename ");
            sb.Append("'");
            if (! string.IsNullOrEmpty(SchemaName))
            {
                sb.Append(SchemaName).Append(".");
            }
            sb.Append(TableName).Append(".").Append(ColumnName);
            sb.Append("', ").AppendFormat(QUOTE_VALUE, NewColumnName);

            sb.Append(", 'COLUMN'");

            return sb.ToString();
        }
    }
}