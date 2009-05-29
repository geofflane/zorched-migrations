// Copyright 2008, 2009 Geoff Lane <geoff@zorched.net>
// 
// This file is part of Zorched Migrations a SQL Schema Migration framework for .NET.
// 
// Zorched Migrations is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Zorched Migrations is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

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