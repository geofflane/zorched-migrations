using System.Data;
using System.Text;
using Zorched.Migrations.Framework.Data;
using Zorched.Migrations.Framework.Extensions;

namespace Zorched.Migrations.SqlServer.Data
{
    public class SqlInsertOperation : BaseDataOperation, IInsertOperation
    {
        public void Execute(IDbCommand command)
        {
            command.CommandText = CreateSql();
            Columns.IterateOver(
                (i, c) =>
                {
                    var param = command.CreateParameter();
                    param.ParameterName = string.Format(PARAM_FORMAT, c);
                    param.Value = Values[i];
                    command.Parameters.Add(param);
                });

            command.ExecuteNonQuery();
        }

        public override string CreateSql()
        {
            var sb = new StringBuilder("INSERT INTO ");
            AddTableInfo(sb, SchemaName, TableName);
            sb.Append(" (");

            Columns.ForEach(c => sb.AppendFormat(QUOTE_FORMAT, c).Append(","));
            sb.TrimEnd(',');

            sb.Append(") VALUES (");
            Columns.ForEach(c => sb.AppendFormat(PARAM_FORMAT, c).Append(","));
            sb.TrimEnd(',');

            sb.Append(")");

            return sb.ToString();
        }
    }
}