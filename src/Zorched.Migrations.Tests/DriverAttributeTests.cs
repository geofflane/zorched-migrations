using System;
using System.Data;
using System.Reflection;
using NUnit.Framework;
using Zorched.Migrations.Framework;
using Zorched.Migrations.Framework.Data;
using Zorched.Migrations.Framework.Inspection;
using Zorched.Migrations.Framework.Schema;

namespace Zorched.Migrations.Tests
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
            var t = DriverAttribute.GetDriver(Assembly.GetAssembly(GetType()));
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

        public IDbParams Database { get; private set; }

        public string DriverName { get; private set; }

        public void Run<T>(Action<T> fn) where T : IOperation
        {
            throw new System.NotImplementedException();
        }

        public void Run(IOperation op)
        {
            throw new System.NotImplementedException();
        }

        public IDataReader Read<T>(Action<T> fn) where T : IReaderOperation
        {
            throw new System.NotImplementedException();
        }

        public IDataReader Read(IReaderOperation op)
        {
            throw new System.NotImplementedException();
        }

        public bool Inspect<T>(Action<T> op) where T : IInspectionOperation
        {
            throw new System.NotImplementedException();
        }

        public bool Inspect(IInspectionOperation op)
        {
            throw new System.NotImplementedException();
        }

        public void AddColumn(Action<IAddColumnOperation> fn)
        {
            throw new System.NotImplementedException();
        }

        public void AddForeignKey(Action<IAddForeignKeyOperation> fn)
        {
            throw new System.NotImplementedException();
        }

        public void AddTable(Action<IAddTableOperation> fn)
        {
            throw new System.NotImplementedException();
        }

        public void Drop(Action<IDropTableOperation> fn)
        {
            throw new System.NotImplementedException();
        }

        public void DropColumn(Action<IDropColumnOperation> fn)
        {
            throw new System.NotImplementedException();
        }

        public void DropConstraint(Action<IDropConstraintOperation> fn)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(Action<IDeleteOperation> fn)
        {
            throw new System.NotImplementedException();
        }

        public void Insert(Action<IInsertOperation> fn)
        {
            throw new System.NotImplementedException();
        }

        public void Update(Action<IUpdateOperation> fn)
        {
            throw new System.NotImplementedException();
        }

        public IDataReader Select(Action<ISelectOperation> fn)
        {
            throw new System.NotImplementedException();
        }

        public void BeforeUp(long version)
        {
            throw new System.NotImplementedException();
        }

        public void BeforeDown(long version)
        {
            throw new System.NotImplementedException();
        }

        public void AfterUp(long version)
        {
            throw new System.NotImplementedException();
        }

        public void AfterDown(long version)
        {
            throw new System.NotImplementedException();
        }
    }
}