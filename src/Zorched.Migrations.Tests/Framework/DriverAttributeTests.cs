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
using System.Data;
using System.Reflection;
using NUnit.Framework;
using Zorched.Migrations.Framework;
using Zorched.Migrations.Framework.Data;
using Zorched.Migrations.Framework.Inspection;
using Zorched.Migrations.Framework.Schema;

namespace Zorched.Migrations.Tests.Framework
{
    [TestFixture]
    public class DriverAttributeTests
    {
        [Test]
        public void driver_can_find_driver_name()
        {
            Assert.AreEqual("Test", DriverAttribute.GetDriverName(typeof (DriverTestClass)));
        }

        [Test]
        public void driver_can_find_provider()
        {
            Assert.AreEqual("System.Data.SqlClient", DriverAttribute.GetProvider(typeof (DriverTestClass)));
        }

        [Test]
        public void driver_can_get_driver_instance()
        {
            Type t = DriverAttribute.GetDriver(Assembly.GetAssembly(GetType()));
            Assert.IsNotNull(t);
            Assert.AreEqual("Test", DriverAttribute.GetDriverName(t));
        }
    }

    [Driver("Test", "System.Data.SqlClient")]
    public class DriverTestClass : IDriver
    {
        public DriverTestClass(IDbParams dbParams, ILogger logger)
        {
            Database = dbParams;
        }

        #region IDriver Members

        public IDbParams Database { get; private set; }

        public string DriverName { get; private set; }

        public void Run<T>(Action<T> fn) where T : IOperation
        {
            throw new NotImplementedException();
        }

        public T NewInstance<T>()
        {
            throw new NotImplementedException();
        }

        public void Run(IOperation op)
        {
            throw new NotImplementedException();
        }

        public IDataReader Read<T>(Action<T> fn) where T : IReaderOperation
        {
            throw new NotImplementedException();
        }

        public IDataReader Select(ISelectOperation op)
        {
            throw new NotImplementedException();
        }

        public IDataReader Read(IReaderOperation op)
        {
            throw new NotImplementedException();
        }

        public bool Inspect<T>(Action<T> op) where T : IInspectionOperation
        {
            throw new NotImplementedException();
        }

        public bool Inspect(IInspectionOperation op)
        {
            throw new NotImplementedException();
        }

        public void AddColumn(Action<IAddColumnOperation> fn)
        {
            throw new NotImplementedException();
        }

        public void AddForeignKey(Action<IAddForeignKeyOperation> fn)
        {
            throw new NotImplementedException();
        }

        public void AddTable(Action<IAddTableOperation> fn)
        {
            throw new NotImplementedException();
        }

        public void Drop(Action<IDropTableOperation> fn)
        {
            throw new NotImplementedException();
        }

        public void DropColumn(Action<IDropColumnOperation> fn)
        {
            throw new NotImplementedException();
        }

        public void DropConstraint(Action<IDropConstraintOperation> fn)
        {
            throw new NotImplementedException();
        }

        public void Delete(Action<IDeleteOperation> fn)
        {
            throw new NotImplementedException();
        }

        public void Insert(Action<IInsertOperation> fn)
        {
            throw new NotImplementedException();
        }

        public void Update(Action<IUpdateOperation> fn)
        {
            throw new NotImplementedException();
        }

        public IDataReader Select(Action<ISelectOperation> fn)
        {
            throw new NotImplementedException();
        }

        public void BeforeUp(long version)
        {
            throw new NotImplementedException();
        }

        public void BeforeDown(long version)
        {
            throw new NotImplementedException();
        }

        public void AfterUp(long version)
        {
            throw new NotImplementedException();
        }

        public void AfterDown(long version)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}