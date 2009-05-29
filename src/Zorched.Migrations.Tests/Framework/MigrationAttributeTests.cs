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

using System.Linq;
using System.Reflection;
using NUnit.Framework;
using Zorched.Migrations.Framework;

namespace Zorched.Migrations.Tests.Framework
{
    [TestFixture]
    public class MigrationAttributeTests
    {
        [Test]
        public void migration_can_get_version()
        {
            Assert.AreEqual(1, MigrationAttribute.GetVersion(typeof(TestMigration1)));
        }

        [Test]
        public void migration_can_find_migrations()
        {
            var migrations = MigrationAttribute.GetTypes(Assembly.GetAssembly(GetType())).ToList();
            Assert.IsNotNull(migrations);
            CollectionAssert.AllItemsAreNotNull(migrations);
        }
    }

    [Migration(1)]
    public class TestMigration1
    {

    }

    [Migration(2)]
    public class TestMigration2
    {

    }
}