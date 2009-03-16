using System;
using System.Data;
using System.Text;
using Zorched.Migrations.Framework;
using Zorched.Migrations.Framework.Data;
using Zorched.Migrations.Framework.Extensions;

namespace Zorched.Migrations.SqlServer.Data
{
    public class SqlUpdateOperation : BaseDataOperation, IUpdateOperation
    {
        public const string UPDATE_FORMAT = QUOTE_FORMAT + "=" + PARAM_FORMAT;

        private readonly WhereHelper whereHelper = new WhereHelper(QUOTE_FORMAT, PARAM_FORMAT);

        public void Where(params Restriction[] restrictions) { whereHelper.Where(restrictions); }
        public void Where(string rawClause) { whereHelper.Where(rawClause); }
        public void Where(string column, object val) { whereHelper.Where(column, val); }

        public void Execute(IDbCommand command)
        {
            command.CommandText = ToString();
            whereHelper.Command = command;
            Columns.IterateOver(
                (i, c) =>
                    {
                        var param = command.CreateParameter();
                        param.ParameterName = string.Format(PARAM_FORMAT, c);
                        param.Value = Values[i] ?? DBNull.Value;
                        command.Parameters.Add(param);
                    });

            whereHelper.AppendValues();
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

            whereHelper.ClauseBuilder = sb;
            whereHelper.AppendWhere();

            return sb.ToString();
        }
    }
}