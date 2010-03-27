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

using System;
using System.Collections.Generic;
using System.Data;
using Zorched.Migrations.Framework;
using Zorched.Migrations.Framework.Data;
using Zorched.Migrations.Framework.Inspection;
using Zorched.Migrations.Framework.Schema;
using Zorched.Migrations.SqlServer.Data;
using Zorched.Migrations.SqlServer.Inspection;
using Zorched.Migrations.SqlServer.Schema;

namespace Zorched.Migrations.SqlServer
{
    [Driver("SQLServer", "System.Data.SqlClient")]
    public class SqlServerDriver : IDriver, IOperationRepository
    {
        public SqlServerDriver(IDbParams dbParams, ILogger logger)
        {
            Database = dbParams;
            Logger = logger;
            RegisteredTypes = new Dictionary<Type, Type>();

            Register<IAddTableOperation>(typeof (SqlAddTableOperation));
            Register<IAddColumnOperation>(typeof (SqlAddColumnOperation));
            Register<IAddForeignKeyOperation>(typeof(SqlAddForeignKeyOperation));
            Register<IChangeColumnOperation>(typeof(SqlChangeColumnOperation));
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

            RegisterReader<IGenericReaderOperation>(typeof (SqlReaderOperation));
            RegisterReader<ISelectOperation>(typeof (SqlSelectOperation));

            RegisterInspector<ITableExistsOperation>(typeof(SqlTableExistsOperation));
            RegisterInspector<IColumnExistsOperation>(typeof(SqlColumnExistsOperation));
        }

        public IDbParams Database { get; set; }
        public ILogger Logger { get; set; }

        public string DriverName { get { return "SQLServer"; } }

        public Dictionary<Type, Type> RegisteredTypes { get; protected set; }

        public void Register<T>(Type impl) where T : IOperation
        {
            RegisteredTypes[typeof (T)] = impl;
        }

        public void RegisterReader<T>(Type impl) where T : IReaderOperation
        {
            RegisteredTypes[typeof (T)] = impl;
        }

        public void RegisterInspector<T>(Type impl) where T : IInspectionOperation
        {
            RegisteredTypes[typeof(T)] = impl;
        }

        public Type TypeForInterface<T>()
        {
            var t = RegisteredTypes[typeof (T)];
            if (null == t)
                throw new OperationNotSupportedException("No such registered implementation for type.", typeof(T));

            return t;
        }

        public T InstanceForInteface<T>()
        {
            Type t = TypeForInterface<T>();
            return (T) t.GetConstructor(Type.EmptyTypes).Invoke(null);
        }

        public void Run(IOperation op)
        {
            using(var cmd = Database.CreateCommand())
            {
                Logger.LogSql(op.ToString());
                op.Execute(cmd);
            }
        }

        public void Run<T>(Action<T> fn) where T : IOperation
        {
            var op = InstanceForInteface<T>();
            fn(op);
            Run(op);
        }

        public IDataReader Read(IReaderOperation op)
        {
            using (var cmd = Database.CreateCommand())
            {
                Logger.LogSql(op.ToString());
                return op.Execute(cmd);
            }
        }

        public IDataReader Read<T>(Action<T> fn) where T : IReaderOperation
        {
            var op = InstanceForInteface<T>();
            fn(op);
            return Read(op);
        }

        public bool Inspect<T>(Action<T> fn) where T : IInspectionOperation
        {
            var op = InstanceForInteface<T>();
            fn(op);
            return Inspect(op);
        }

        public bool Inspect(IInspectionOperation op)
        {
            using (var cmd = Database.CreateCommand())
            {
                Logger.LogSql(op.ToString());
                return op.Execute(cmd);
            }
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
            Logger.LogInfo(String.Format("Migrating Up: {0}", version));
        }

        public void BeforeDown(long version)
        {
            Logger.LogInfo(String.Format("Migrating Down: {0}", version));
        }

        public void AfterUp(long version)
        {
            Logger.LogInfo(String.Format("Applied: {0}", version));
        }

        public void AfterDown(long version)
        {
            Logger.LogInfo(String.Format("Removed: {0}", version));
        }
    }
}