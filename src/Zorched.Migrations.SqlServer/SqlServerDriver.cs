using System;
using System.Collections.Generic;
using System.Data;
using Zorched.Migrations.Framework;
using Zorched.Migrations.Framework.Data;
using Zorched.Migrations.Framework.Schema;
using Zorched.Migrations.SqlServer.Data;
using Zorched.Migrations.SqlServer.Schema;

namespace Zorched.Migrations.SqlServer
{
    [Driver("SQLServer", "System.Data.SqlClient")]
    public class SqlServerDriver : IDriver, IOperationRepository
    {
        public SqlServerDriver(IDbConnection connection)
        {
            Connection = connection;
            RegisteredTypes = new Dictionary<Type, Type>();

            Register<IAddTableOperation>(typeof (SqlAddTableOperation));
            Register<IAddColumnOperation>(typeof (SqlAddColumnOperation));
            Register<IAddForeignKeyOperation>(typeof (SqlAddForeignKeyOperation));
            Register<IDropTableOperation>(typeof (SqlDropTableOperation));
            Register<IDropColumnOperation>(typeof (SqlDropColumnOperation));
            Register<IDropConstraintOperation>(typeof(SqlDropConstraintOperation));
            Register<IRenameTableOperation>(typeof(SqlRenameTableOperation));
            Register<IRenameColumnOperation>(typeof(SqlRenameColumnOperation));
            Register<IAddCheckConstraintOperation>(typeof(SqlAddCheckConstraintOperation));
            Register<IAddUniqueConstraintOperation>(typeof(SqlAddUniqueConstraintOperation));
            Register<IGenericOperation>(typeof (SqlGenericOperation));

            Register<IDeleteOperation>(typeof (SqlDeleteOperation));
            Register<IInsertOperation>(typeof (SqlInsertOperation));
            Register<IUpdateOperation>(typeof (SqlUpdateOperation));

            RegisterReader<IReaderOperation>(typeof (SqlReaderOperation));
            RegisterReader<ISelectOperation>(typeof (SqlSelectOperation));
        }

        public IDbConnection Connection { get; set; }

        public string DriverName { get { return "SQLServer"; } }

        public Dictionary<Type, Type> RegisteredTypes { get; protected set; }

        public void Run(IOperation op)
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

        public void Run<T>(Action<T> fn) where T : IOperation
        {
            var op = InstanceForInteface<T>();
            fn(op);
            Run(op);
        }

        public void Register<T>(Type impl) where T : IOperation
        {
            RegisteredTypes[typeof (T)] = impl;
        }

        public void RegisterReader<T>(Type impl) where T : IReaderOperation
        {
            RegisteredTypes[typeof (T)] = impl;
        }

        public Type TypeForInterface<T>()
        {
            var t = RegisteredTypes[typeof (T)];
            if (null == t)
                throw new Exception("No such registered implementation for type.");

            return t;
        }

        public T InstanceForInteface<T>()
        {
            Type t = TypeForInterface<T>();
            return (T) t.GetConstructor(Type.EmptyTypes).Invoke(null);
        }

        public IDataReader Read(IReaderOperation op)
        {
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

        public IDataReader Read<T>(Action<T> fn) where T : IReaderOperation
        {
            var op = InstanceForInteface<T>();
            fn(op);
            return Read(op);
        }

        public IDataReader Select(Action<ISelectOperation> fn)
        {
            return Read(fn);
        }


        public void AddColumn(Action<IAddColumnOperation> fn)
        {
            Run(fn);
        }

        public void AddForeignKey(Action<IAddForeignKeyOperation> fn)
        {
            Run(fn);
        }

        public void AddTable(Action<IAddTableOperation> fn)
        {
            Run(fn);
        }

        public void Drop(Action<IDropTableOperation> fn)
        {
            Run(fn);
        }

        public void DropColumn(Action<IDropColumnOperation> fn)
        {
            Run(fn);
        }

        public void DropConstraint(Action<IDropConstraintOperation> fn)
        {
            Run(fn);
        }

        public void Delete(Action<IDeleteOperation> fn)
        {
            Run(fn);
        }

        public void Insert(Action<IInsertOperation> fn)
        {
            Run(fn);
        }

        public void Update(Action<IUpdateOperation> fn)
        {
            Run(fn);
        }

        public void BeforeUp(long version)
        {
        }

        public void BeforeDown(long version)
        {
        }

        public void AfterUp(long version)
        {
        }

        public void AfterDown(long version)
        {
        }
    }
}