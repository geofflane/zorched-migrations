using System;
using System.Data;
using Zorched.Migrations.Framework;
using Zorched.Migrations.Framework.Schema;
using Zorched.Migrations.Providers.SQLServer.Schema;

namespace Zorched.Migrations.Providers.SQLServer
{
    [Driver("SQLServer")]
    public class SqlServerDriver : IDriver
    {
        public SqlServerDriver(IDbConnection connection)
        {
            Connection = connection;
        }

        public IDbConnection Connection { get; set; }

        public void ChangeSchema(ISchemaOperation op)
        {
            var cmd = Connection.CreateCommand();
            try
            {
                op.Execute(cmd);
            }
            finally
            {
                cmd.Dispose();
            }
        }

        public void ChangeSchema<T>(Action<T> fn, T op) where T : ISchemaOperation
        {
            fn(op);
            ChangeSchema(op);
        }

        public void Drop(Action<IDropTableOperation> fn)
        {
            ChangeSchema(fn, new SqlDropTableOperation());
        }

        public void AddTable(Action<IAddTableOperation> fn)
        {
            ChangeSchema(fn, new SqlAddTableOperation());
        }
    }
}