using System;
using System.Data;
using Zorched.Migrations.Framework;
using Zorched.Migrations.Framework.Data;
using Zorched.Migrations.Framework.Schema;
using Zorched.Migrations.SqlServer.Data;
using Zorched.Migrations.SqlServer.Schema;

namespace Zorched.Migrations.SqlServer
{
    [Driver("SQLServer", "System.Data.SqlClient")]
    public class SqlServerDriver : IDriver
    {
        public SqlServerDriver(IDbConnection connection)
        {
            Connection = connection;
        }

        public IDbConnection Connection { get; set; }

        public void Execute(IOperation op)
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

        public void Execute<T>(Action<T> fn, T op) where T : IOperation
        {
            fn(op);
            Execute(op);
        }

        public void AddColumn(Action<IAddColumnOperation> fn)
        {
            Execute(fn, new SqlAddColumnOperation());
        }

        public void AddForeignKey(Action<IAddForeignKeyOperation> fn)
        {
            Execute(fn, new SqlAddForeignKeyOperation());
        }

        public void AddTable(Action<IAddTableOperation> fn)
        {
            Execute(fn, new SqlAddTableOperation());
        }

        public void Drop(Action<IDropTableOperation> fn)
        {
            Execute(fn, new SqlDropTableOperation());
        }

        public void DropColumn(Action<IDropColumnOperation> fn)
        {
            Execute(fn, new SqlDropColumnOperation());
        }

        public void DropConstraint(Action<IDropConstraintOperation> fn)
        {
            Execute(fn, new SqlDropConstraintOperation());
        }

        public void Delete(Action<IDeleteOperation> fn)
        {
            Execute(fn, new SqlDeleteOperation());
        }

        public void Insert(Action<IInsertOperation> fn)
        {
            Execute(fn, new SqlInsertOperation());
        }

        public void Update(Action<IUpdateOperation> fn)
        {
            Execute(fn, new SqlUpdateOperation());
        }

        public IDataReader Select(Action<ISelectOperation> fn)
        {
            var op = new SqlSelectOperation();
            var cmd = Connection.CreateCommand();
            try
            {
                return op.Execute(cmd);
            }
            finally
            {
                cmd.Dispose();
            }
        }

        public void Execute(Action<IGenericOperation> fn)
        {
            var cmd = Connection.CreateCommand();
            try
            {
                var op = new SqlGenericOperation();
                fn(op);
                op.Execute(cmd);
            }
            finally
            {
                cmd.Dispose();
            }
        }
    }
}