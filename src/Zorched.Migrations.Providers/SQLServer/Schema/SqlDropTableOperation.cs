using System.Data;
using System.Text;
using Zorched.Migrations.Framework.Schema;

namespace Zorched.Migrations.Providers.SQLServer.Schema
{
    class SqlDropTableOperation : IDropTableOperation
    {
        public SqlDropTableOperation()
        {
            SchemaName = "dbo";
        }

        public string SchemaName { get; set; }

        public string TableName { get; set; }

        public void Execute(IDbCommand cmd)
        {
            cmd.CommandText = DropCommand();
            cmd.ExecuteNonQuery();
        }

        public virtual string DropCommand()
        {
            var sb = new StringBuilder("DROP TABLE ");
            if (!string.IsNullOrEmpty(SchemaName))
            {
                sb.Append(SchemaName).Append(".");
            }
            sb.Append(TableName);

            return sb.ToString();
        }
    }
}