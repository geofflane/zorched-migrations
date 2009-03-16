using System;
using System.Data;
using System.Text;
using Zorched.Migrations.Framework;
using Zorched.Migrations.Framework.Data;

namespace Zorched.Migrations.SqlServer.Data
{
    public class SqlDeleteOperation : BaseDataOperation, IDeleteOperation
    {
        private readonly WhereHelper whereHelper = new WhereHelper(QUOTE_FORMAT, PARAM_FORMAT);

        public void Where(params Restriction[] restrictions) { whereHelper.Where(restrictions); }
        public void Where(string rawClause) { whereHelper.Where(rawClause); }
        public void Where(string column, object val) { whereHelper.Where(column, val); }

        public void Execute(IDbCommand command)
        {
            command.CommandText = ToString();
            whereHelper.Command = command;
            whereHelper.AppendValues();
            command.ExecuteNonQuery();
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(TableName))
                throw new ArgumentException("TableName must be set.");

            var sb = new StringBuilder("DELETE FROM ");
            AddTableInfo(sb, SchemaName, TableName);

            whereHelper.ClauseBuilder = sb;
            whereHelper.AppendWhere();

            return sb.ToString();
        }
    }
}