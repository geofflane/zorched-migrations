using System.Data;
using System.Text;
using Zorched.Migrations.Framework.Inspection;
using Zorched.Migrations.SqlServer.Data;

namespace Zorched.Migrations.SqlServer.Inspection
{
    public class SqlColumnExistsOperation : IColumnExistsOperation
    {
        public string SchemaName { get; set; }
        public string TableName { get; set; }
        public string ColumnName { get; set; }

        public bool Execute(IDbCommand command)
        {
            command.CommandText = ToString();
            var tableParam = command.CreateParameter();
            tableParam.ParameterName = "@tableName";
            tableParam.Value = TableName;
            command.Parameters.Add(tableParam);

            var columnParam = command.CreateParameter();
            columnParam.ParameterName = "@columnName";
            columnParam.Value = ColumnName;
            command.Parameters.Add(columnParam);

            if (! string.IsNullOrEmpty(SchemaName))
            {
                var schemaParam = command.CreateParameter();
                schemaParam.ParameterName = "@schemaName";
                schemaParam.Value = SchemaName;
                command.Parameters.Add(schemaParam);
            }

            using (var reader = command.ExecuteReader())
            {
                return reader.Read();
            }
        }

        public string WhereClause()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("TABLE_NAME=@tableName AND COLUMN_NAME=@columnName");
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
                TableName = "COLUMNS"
            };
            select.Columns.Add("TABLE_NAME");
            select.WhereClause = WhereClause();

            return select.ToString();
        }
    }
}