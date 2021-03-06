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
using NUnit.Framework;
using Rhino.Mocks;
using Zorched.Migrations.Core;
using Zorched.Migrations.Framework;
using Zorched.Migrations.Framework.Schema;
using Zorched.Migrations.Framework.Simple;

namespace Zorched.Migrations.Tests.Framework
{
    [TestFixture]
    public class MigrationTests
    {
        private readonly SetupRunner setupRunner = new SetupRunner();

        private MockRepository mocks;
        private IOperationRepository opRepos;
        private IDriver driver;
        private ISchemaInfo schemaInfo;

        [SetUp]
        public void Setup()
        {
            mocks = new MockRepository();
            opRepos = mocks.StrictMock<IOperationRepository>();
            driver = mocks.StrictMock<IDriver>();
            schemaInfo = mocks.StrictMock<ISchemaInfo>();
        }

        [TearDown]
        public void Teardown()
        {
            mocks.BackToRecordAll();
        }

        [Test]
        public void can_create_instance_with_type()
        {
            var migration = new Migration(typeof(MigrationTestType));
            Assert.IsNotNull(migration);
            Assert.AreEqual(4, migration.Version);
        }

        [Test]
        public void can_run_setup_method_when_setup_exists()
        {
            var migration = new Migration(typeof(MigrationTestType));
            Assert.IsNotNull(migration);

            Expect.Call(() => opRepos.Register<IGenericOperation>(typeof(String)));
            mocks.ReplayAll();

            migration.Setup(setupRunner, new TestLogger(), opRepos);

            mocks.VerifyAll();
        }

        [Test]
        public void can_run_up_method()
        {
            var migration = new Migration(typeof(MigrationTestType));
            Assert.IsNotNull(migration);

            Expect.Call(() => driver.BeforeUp(4));
            Expect.Call(() => driver.Run(Arg<Action<IGenericOperation>>.Is.Anything));
            Expect.Call(driver.DriverName).Return("TestDriver");

            Expect.Call(() => driver.AfterUp(4));

            Expect.Call(() => schemaInfo.InsertSchemaVersion(4, typeof(MigrationTestType).Assembly.GetName().Name, typeof(MigrationTestType).FullName));
            mocks.ReplayAll();

            migration.Up(driver, new TestLogger(), schemaInfo);

            mocks.VerifyAll();
        }

        [Test]
        public void can_run_down_method()
        {
            var migration = new Migration(typeof(MigrationTestType));
            Assert.IsNotNull(migration);

            Expect.Call(() => driver.BeforeDown(4));
            Expect.Call(() => driver.Run(Arg<Action<IGenericOperation>>.Is.Anything));
            Expect.Call(driver.DriverName).Return("TestDriver");
            Expect.Call(() => driver.AfterDown(4));

            Expect.Call(() => schemaInfo.DeleteSchemaVersion(4, typeof(MigrationTestType).Assembly.GetName().Name));
            mocks.ReplayAll();

            migration.Down(driver, new TestLogger(), schemaInfo);

            mocks.VerifyAll();
        }

        [Test]
        public void can_run_setup_method_when_setup_does_not_exist()
        {
            var migration = new Migration(typeof(MigrationTestType2));
            Assert.IsNotNull(migration);
            mocks.ReplayAll();

            migration.Setup(setupRunner, new TestLogger(), opRepos);

            mocks.VerifyAll();
        }
    }

    public class TestGenericOperation : IGenericOperation
    {
        public void Execute(IDbCommand command)
        {
            throw new NotImplementedException();
        }

        public string Sql { get; set; }
    }

    [Migration(4)]
    public class MigrationTestType
    {
        [Setup]
        public void Setup(IOperationRepository repository)
        {
            repository.Register<IGenericOperation>(typeof(String));
        }

        [Up]
        public void DoSomething(ActionRunner runner)
        {
            runner.Driver.Run<IGenericOperation>(
                op => { op.Sql = "Foo"; }
                );
        }

        [Down]
        public void DoSomethingElse(ActionRunner runner)
        {
            runner.Driver.Run<IGenericOperation>(
                op => { op.Sql = "Foo"; }
                );
        }
    }

    [Migration(5)]
    public class MigrationTestType2
    {

        [Up]
        public void DoSomething(ActionRunner runner)
        {
            runner.Driver.Run<IGenericOperation>(
                op => { op.Sql = "Foo"; }
                );
        }

        [Down]
        public void DoSomethingElse(ActionRunner runner)
        {
            runner.Driver.Run<IGenericOperation>(
                op => { op.Sql = "Foo"; }
                );
        }
    }
}