using System;
using System.Collections.Generic;
using System.Data;
using Zorched.Migrations.Framework.Data;
using Zorched.Migrations.Framework.Inspection;

namespace Zorched.Migrations.Framework
{
    /// <summary>
    /// Base implementation of the Driver interface to make it easier to implement Drivers for specific databases.
    /// </summary>
    public class AbstractDriver : IDriver
    {
        public IDbParams Database { get; set; }
        public ILogger Logger { get; set; }

        public string DriverName { get { return "SQLServer"; } }

        protected Dictionary<Type, Type> RegisteredTypes { get; set; }

        public AbstractDriver(IDbParams dbParams, ILogger logger)
        {
            Database = dbParams;
            Logger = logger;
            RegisteredTypes = new Dictionary<Type, Type>();
        }

        public void Register<T>(Type impl) where T : IOperation
        {
            RegisteredTypes[typeof(T)] = impl;
        }

        public void RegisterReader<T>(Type impl) where T : IReaderOperation
        {
            RegisteredTypes[typeof(T)] = impl;
        }

        public void RegisterInspector<T>(Type impl) where T : IInspectionOperation
        {
            RegisteredTypes[typeof(T)] = impl;
        }

        public virtual Type TypeForInterface<T>()
        {
            var t = RegisteredTypes[typeof(T)];
            if (null == t)
                throw new OperationNotSupportedException("No such registered implementation for type.", typeof(T));

            return t;
        }

        public virtual T NewInstance<T>()
        {
            Type t = TypeForInterface<T>();
            return (T)t.GetConstructor(Type.EmptyTypes).Invoke(null);
        }

        public virtual void Run(IOperation op)
        {
            using (var cmd = Database.CreateCommand())
            {
                Logger.LogSql(op.ToString());
                op.Execute(cmd);
            }
        }

        public virtual void Run<T>(Action<T> fn) where T : IOperation
        {
            var op = NewInstance<T>();
            fn(op);
            Run(op);
        }

        public virtual IDataReader Select(ISelectOperation op)
        {
            return Read(op);
        }

        public virtual IDataReader Read(IReaderOperation op)
        {
            using (var cmd = Database.CreateCommand())
            {
                Logger.LogSql(op.ToString());
                return op.Execute(cmd);
            }
        }

        public virtual IDataReader Read<T>(Action<T> fn) where T : IReaderOperation
        {
            var op = NewInstance<T>();
            fn(op);
            return Read(op);
        }

        public virtual bool Inspect<T>(Action<T> fn) where T : IInspectionOperation
        {
            var op = NewInstance<T>();
            fn(op);
            return Inspect(op);
        }

        public virtual bool Inspect(IInspectionOperation op)
        {
            using (var cmd = Database.CreateCommand())
            {
                Logger.LogSql(op.ToString());
                return op.Execute(cmd);
            }
        }

        public virtual IDataReader Select(Action<ISelectOperation> fn)
        {
            return Read(fn);
        }

        public virtual void BeforeUp(long version)
        {
            Logger.LogInfo(String.Format("Migrating Up: {0}", version));
        }

        public virtual void BeforeDown(long version)
        {
            Logger.LogInfo(String.Format("Migrating Down: {0}", version));
        }

        public virtual void AfterUp(long version)
        {
            Logger.LogInfo(String.Format("Applied: {0}", version));
        }

        public virtual void AfterDown(long version)
        {
            Logger.LogInfo(String.Format("Removed: {0}", version));
        }
    }
}