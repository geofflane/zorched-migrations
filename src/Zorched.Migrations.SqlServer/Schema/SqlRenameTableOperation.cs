using System.Text;
using Zorched.Migrations.Framework.Schema;

namespace Zorched.Migrations.SqlServer.Schema
{
    public class SqlRenameTableOperation : BaseSchemaOperation, IRenameTableOperation
    {
        private const string QUOTE_VALUE = "'{0}'";

        public string NewTableName { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder("EXEC sp_rename ");
            sb.Append("'");
            if (! string.IsNullOrEmpty(SchemaName))
            {
                sb.Append(SchemaName).Append(".");
            }
            sb.Append(TableName);
            sb.Append("', ").AppendFormat(QUOTE_VALUE, NewTableName);

            return sb.ToString();
        }
    }
}