using System.Data;
using Zorched.Migrations.Framework.Inspection;
using Zorched.Migrations.SqlServer.Data;

namespace Zorched.Migrations.SqlServer.Inspection
{
    public class SqlTableExistsOperation : ITableExistsOperation
    {
        private readonly SqlSelectOperation SELECT_OP = new SqlSelectOperation
                                                            {
                                                                SchemaName = "INFORMATION_SCHEMA",
                                                                TableName = "TABLES"
                                                            };
        public string SchemaName { get; set; }
        public string TableName { get; set; }

        public bool Execute(IDbCommand command)
        {
            using (var reader = SELECT_OP.Execute(command))
            {
                return reader.Read();
            }
        }

        public override string ToString()
        {
            SELECT_OP.Columns.Add("TABLE_NAME");
            SELECT_OP.Where("TABLE_NAME", TableName);
            if (!string.IsNullOrEmpty(SchemaName))
            {
                SELECT_OP.Where("TABLE_SCHEMA", SchemaName);
            }

            return SELECT_OP.ToString();
        }
    }
}