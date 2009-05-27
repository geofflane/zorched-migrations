using System.Data;
using Zorched.Migrations.Framework;

namespace Zorched.Migrations.Core
{
    /// <summary>
    /// Default implementation of IDbParams.
    /// </summary>
    public class DbParams : IDbParams
    {
        private const int DEFAULT_TIMEOUNT = 30;

        private int commandTimeout;

        /// <summary>
        /// Create a new DbParams Object
        /// </summary>
        /// <param name="connection">The database connection to use.</param>
        /// <param name="commandTimeout">The timeout to use for Commands in seconds. Defaults to 30 seconds.</param>
        public DbParams(IDbConnection connection, int commandTimeout)
        {
            Connection = connection;
            if (Connection.State != ConnectionState.Open)
            {
                Connection.Open();
            }

            CommandTimeout = commandTimeout;
        }

        public DbParams(IDbConnection connection) : this(connection, DEFAULT_TIMEOUNT)
        {
        }

        public int CommandTimeout
        {
            get { return commandTimeout; }
            set { commandTimeout = 0 < value ? value : DEFAULT_TIMEOUNT; }
        }
        
        public IDbConnection Connection { get; set; }
        
        public IDbTransaction Transaction { get; set; }

        public IDbCommand CreateCommand()
        {
            var cmd = Connection.CreateCommand();
            if (null != Transaction)
                cmd.Transaction = Transaction;

            cmd.CommandTimeout = CommandTimeout;
            return cmd;
        }

        public IDbTransaction BeginTransaction()
        {
            Transaction = Connection.BeginTransaction();
            return Transaction;
        }
    }
}