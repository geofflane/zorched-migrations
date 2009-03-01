using System;
using System.Data;
using System.Text;
using Zorched.Migrations.Framework.Data;
using Zorched.Migrations.Framework.Extensions;

namespace Zorched.Migrations.SqlServer.Data
{
    public class SqlUpdateOperation : BaseDataOperation, IUpdateOperation
    {
        public const string UPDATE_FORMAT = QUOTE_FORMAT + "=" + PARAM_FORMAT;

        private readonly WhereHelper whereHelper = new WhereHelper();

        public string WhereColumn { get { return whereHelper.WhereColumn; } set { whereHelper.WhereColumn = value; } }
        public object WhereValue { get { return whereHelper.WhereValue; } set { whereHelper.WhereValue = value; } }
        public string WhereClause { get { return whereHelper.WhereClause; } set { whereHelper.WhereClause = value; } }

        public void Execute(IDbCommand command)
        {
            command.CommandText = ToString();
            Columns.IterateOver(
                (i, c) =>
                    {
                        var param = command.CreateParameter();
                        param.ParameterName = string.Format(PARAM_FORMAT, c);
                        param.Value = Values[i] ?? DBNull.Value;
                        command.Parameters.Add(param);
                    });

            whereHelper.AppendWhereParameter(command, PARAM_FORMAT);
            command.ExecuteNonQuery();
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(TableName))
                throw new ArgumentException("TableName must be set.");

            var sb = new StringBuilder("UPDATE ");
            AddTableInfo(sb, SchemaName, TableName);
            sb.Append(" SET ");

            Columns.IterateOver((i, c) => sb.AppendFormat(UPDATE_FORMAT, c).Append(","));
            sb.TrimEnd(',');

            whereHelper.AppendWhere(sb, UPDATE_FORMAT);

            return sb.ToString();
        }
    }
}