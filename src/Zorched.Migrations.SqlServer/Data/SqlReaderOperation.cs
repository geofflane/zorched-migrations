using System.Data;
using Zorched.Migrations.Framework.Data;

namespace Zorched.Migrations.SqlServer.Data
{
    public class SqlReaderOperation : SqlBaseOperation, IReaderOperation
    {
        public string Sql { get; set; }

        public IDataReader Execute(IDbCommand cmd)
        {
            cmd.CommandText = Sql;
            return cmd.ExecuteReader();
        }

        public override string ToString()
        {
            return Sql;
        }
    }
}