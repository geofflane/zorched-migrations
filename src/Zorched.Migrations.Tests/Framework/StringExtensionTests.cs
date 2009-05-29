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

using System.Text;
using NUnit.Framework;
using Zorched.Migrations.Framework.Extensions;

namespace Zorched.Migrations.Tests.Framework
{
    [TestFixture]
    public class StringExtensionTests
    {

        [Test]
        public void can_find_endwith_of_stringbuffer()
        {
            var sb1 = new StringBuilder("foo1234");
            var sb2 = new StringBuilder("foo1233");

            Assert.IsTrue(sb1.EndsWith('4'));
            Assert.IsFalse(sb2.EndsWith('4'));
        }

        [Test]
        public void can_trim_character_end_of_stringbuffer()
        {
            var sb1 = new StringBuilder("foo1234");
            var sb2 = new StringBuilder("foo1233");
            Assert.AreEqual("foo123", sb1.TrimEnd('4').ToString());
            Assert.AreEqual("foo1233", sb2.TrimEnd('4').ToString());
        }

        [Test]
        public void can_trim_end_of_stringbuffer()
        {
            var sb1 = new StringBuilder("foo1234   \r\n");
            var sb2 = new StringBuilder("foo1234");
            Assert.AreEqual("foo1234", sb1.TrimEnd().ToString());
            Assert.AreEqual("foo1234", sb2.TrimEnd().ToString());
        }

        [Test]
        public void can_generate_human_name()
        {
            Assert.AreEqual("Create a table", "CreateATable".ToHumanName());
            Assert.AreEqual("Add a column to a table", "AddAColumnToATable".ToHumanName());
            Assert.AreEqual("addacolumn", "addacolumn".ToHumanName());
            Assert.AreEqual("add a column", "addAColumn".ToHumanName());
        }
    }
}