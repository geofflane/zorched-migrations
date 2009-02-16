using System.Text;

namespace Zorched.Migrations.SqlServer.Data
{
    public class WhereHelper
    {
        public string WhereColumn { get; set; }
        public object WhereValue { get; set; }
        public string WhereClause { get; set; }

        public void AppendWhere(StringBuilder sb, string updateFormat)
        {
            if (NeedsWhereClause)
            {
                sb.Append(" WHERE ");
                if (!string.IsNullOrEmpty(WhereColumn))
                {
                    sb.AppendFormat(updateFormat, WhereColumn);
                }
                if (!string.IsNullOrEmpty(WhereClause))
                {
                    sb.Append(" ").Append(WhereClause);
                }
            }
        }

        public bool NeedsWhereClause
        {
            get { return !string.IsNullOrEmpty(WhereClause) || HasWhereColumn; }
        }

        public bool HasWhereColumn
        {
            get
            {
                return !string.IsNullOrEmpty(WhereColumn) && null != WhereValue;
            }
        }
    }
}