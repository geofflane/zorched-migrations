using System.Data;
using System.Text;
using Zorched.Migrations.Framework.Inspection;
using Zorched.Migrations.SqlServer.Data;

namespace Zorched.Migrations.SqlServer.Inspection
{
    public class SqlTableExistsOperation : ITableExistsOperation
    {
        public string SchemaName { get; set; }
        public string TableName { get; set; }

        public bool Execute(IDbCommand command)
        {
            command.CommandText = ToString();
            var tableParam = command.CreateParameter();
            tableParam.ParameterName = "@tableName";
            tableParam.Value = TableName;
            command.Parameters.Add(tableParam);

            if (! string.IsNullOrEmpty(SchemaName))
            {
                var schemaParam = command.CreateParameter();
                schemaParam.ParameterName = "@schemaName";
                schemaParam.Value = SchemaName;
                command.Parameters.Add(schemaParam);
            }

            IDataReader reader = command.ExecuteReader();
            return reader.Read();
        }

        public string WhereClause()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("TABLE_NAME=@tableName");
            if (! string.IsNullOrEmpty(SchemaName))
            {
                sb.AppendFormat(" AND TABLE_SCHEMA=@schemaName");
            }

            return sb.ToString();
        }

        public override string ToString()
        {
            var select = new SqlSelectOperation
            {
                SchemaName = "INFORMATION_SCHEMA",
                TableName = "TABLES"
            };
            select.Columns.Add("TABLE_NAME");
            select.WhereClause = WhereClause();

            return select.ToString();
        }
    }
}