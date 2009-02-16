using System.Data;
using Zorched.Migrations.Framework.Schema;

namespace Zorched.Migrations.SqlServer.Schema
{
    public class SqlGenericOperation : IGenericOperation
    {
        public string Sql { get; set; }

        public virtual void Execute(IDbCommand cmd)
        {
            cmd.CommandText = Sql;
            cmd.ExecuteNonQuery();
        }
    }
}