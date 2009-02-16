using System.Text;
using Zorched.Migrations.Framework;

namespace Zorched.Migrations.SqlServer
{
    public abstract class SqlBaseOperation
    {
        protected const string QUOTE_FORMAT = "[{0}]";

        public void AddColumnDefinition(StringBuilder sb, Column c)
        {
            sb.AppendFormat(QUOTE_FORMAT, c.Name);
            sb.Append(" ").Append(DbTypeMap.GetTypeName(c.DbType, c.Size));
            if (c.Property.Match(ColumnProperty.Identity))
            {
                sb.Append(" IDENTITY(1,1)");
            }
            sb.Append(c.Property.Match(ColumnProperty.Null) ? " NULL" : " NOT NULL");
            if (null != c.DefaultValue)
            {
                sb.Append("DEFAULT ").Append(c.DefaultValue);
            }
        }

        public void AddTableInfo(StringBuilder sb, string schemaName, string tableName)
        {
            if (!string.IsNullOrEmpty(schemaName))
            {
                sb.AppendFormat(QUOTE_FORMAT, schemaName).Append(".");
            }
            sb.AppendFormat(QUOTE_FORMAT, tableName);
        }
    }
}