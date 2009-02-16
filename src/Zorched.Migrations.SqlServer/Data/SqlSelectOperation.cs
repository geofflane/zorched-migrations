using System.Collections.Generic;
using System.Data;
using System.Text;
using Zorched.Migrations.Framework.Data;
using Zorched.Migrations.Framework.Extensions;

namespace Zorched.Migrations.SqlServer.Data
{
    public class SqlSelectOperation : SqlBaseOperation, ISelectOperation
    {
        private readonly WhereHelper whereHelper = new WhereHelper();

        public SqlSelectOperation()
        {
            Columns = new List<string>();
        }

        public string SchemaName { get; set; }
        public string TableName { get; set; }

        public IList<string> Columns { get; protected set; }

        public string WhereColumn { get { return whereHelper.WhereColumn; } set { whereHelper.WhereColumn = value; } }
        public object WhereValue { get { return whereHelper.WhereValue; } set { whereHelper.WhereValue = value; } }
        public string WhereClause { get { return whereHelper.WhereClause; } set { whereHelper.WhereClause = value; } }

        public IDataReader Execute(IDbCommand cmd)
        {
            cmd.CommandText = CreateSql();
            return cmd.ExecuteReader();
        }

        public string CreateSql()
        {
            var sb = new StringBuilder("SELECT ");
            if (0 == Columns.Count)
            {
                sb.Append("*");
            } 
            else
            {
                Columns.ForEach(c => sb.AppendFormat(QUOTE_FORMAT, c).Append(","));
                sb.TrimEnd(',');
            }
            sb.Append(" FROM ");
            
            AddTableInfo(sb, SchemaName, TableName);

            whereHelper.AppendWhere(sb, SqlUpdateOperation.UPDATE_FORMAT);

            return sb.ToString();
        }
    }
}