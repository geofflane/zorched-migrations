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

using NUnit.Framework;
using Zorched.Migrations.Framework;

namespace Zorched.Migrations.Tests.Framework
{
    [TestFixture]
    public class OnlyWhenDriverAttributeTests
    {

        [Test]
        public void only_when_attribute_should_run_when_driver_included()
        {
            var mi = typeof(OnlyWhenTestClass).GetMethod("Foo1");
            Assert.IsTrue(OnlyWhenDriverAttribute.ShouldRun(mi, "Test1"));
            Assert.IsTrue(OnlyWhenDriverAttribute.ShouldRun(mi, "Test3"));
        }

        [Test]
        public void only_when_attribute_should_not_run_when_driver_not_included()
        {
            var mi = typeof(OnlyWhenTestClass).GetMethod("Foo1");
            Assert.IsFalse(OnlyWhenDriverAttribute.ShouldRun(mi, "NotThere"));
        }

        [Test]
        public void only_when_should_run_when_no_attribute()
        {
            var mi = typeof(OnlyWhenTestClass).GetMethod("Foo2");
            Assert.IsTrue(OnlyWhenDriverAttribute.ShouldRun(mi, "NotThere"));
        }

        private class OnlyWhenTestClass
        {
            [Down(1)]
            [OnlyWhenDriver("Test1;Test3")]
            public void Foo1()
            {
                
            }

            [Down(2)]
            public void Foo2()
            {

            }
        }
    }
}