using System.Data;
using System.Text;
using Zorched.Migrations.Framework.Data;

namespace Zorched.Migrations.SqlServer.Data
{
    public class SqlDeleteOperation : BaseDataOperation, IDeleteOperation
    {
        private readonly WhereHelper whereHelper = new WhereHelper();

        public string WhereColumn { get { return whereHelper.WhereColumn; } set { whereHelper.WhereColumn = value; } }
        public object WhereValue { get { return whereHelper.WhereValue; } set { whereHelper.WhereValue = value; } }
        public string WhereClause { get { return whereHelper.WhereClause; } set { whereHelper.WhereClause = value; } }

        public void Execute(IDbCommand command)
        {
            command.CommandText = ToString();
            whereHelper.AppendWhereParameter(command, PARAM_FORMAT);
            command.ExecuteNonQuery();
        }

        public override string ToString()
        {
            var sb = new StringBuilder("DELETE FROM ");
            AddTableInfo(sb, SchemaName, TableName);

            whereHelper.AppendWhere(sb, SqlUpdateOperation.UPDATE_FORMAT);

            return sb.ToString();
        }
    }
}