using System.Data;
using Zorched.Migrations.Framework;

namespace Zorched.Migrations.Core
{
    public class DbParams : IDbParams
    {

        public DbParams(IDbConnection connection)
        {
            Connection = connection;
            if (Connection.State != ConnectionState.Open)
            {
                Connection.Open();
            }
        }

        public IDbConnection Connection { get; set; }
        public IDbTransaction Transaction { get; set; }

        public IDbCommand CreateCommand()
        {
            var cmd = Connection.CreateCommand();
            if (null != Transaction)
                cmd.Transaction = Transaction;

            return cmd;
        }

        public IDbTransaction BeginTransaction()
        {
            Transaction = Connection.BeginTransaction();
            return Transaction;
        }
    }
}