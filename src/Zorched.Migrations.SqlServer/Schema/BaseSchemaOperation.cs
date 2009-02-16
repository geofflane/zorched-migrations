using System.Data;

namespace Zorched.Migrations.SqlServer.Schema
{
    public abstract class BaseSchemaOperation : SqlBaseOperation
    {
        public string SchemaName { get; set; }
        public string TableName { get; set; }

        public virtual void Execute(IDbCommand cmd)
        {
            cmd.CommandText = CreateSql();
            cmd.ExecuteNonQuery();
        }

        public abstract string CreateSql();
    }
}
