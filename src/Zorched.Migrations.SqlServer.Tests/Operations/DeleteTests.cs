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
using Zorched.Migrations.SqlServer.Data;

namespace Zorched.Migrations.SqlServer.Tests.Operations
{
    [TestFixture]
    public class DeleteTests
    {
        
        [Test]
        public void delete_with_schema()
        {
            var op = new SqlDeleteOperation {SchemaName = "dbo", TableName = "Foo"};
            Assert.AreEqual("DELETE FROM [dbo].[Foo]", op.ToString());
        }

        [Test]
        public void delete_no_schema()
        {
            var op = new SqlDeleteOperation { SchemaName = null, TableName = "Foo" };
            Assert.AreEqual("DELETE FROM [Foo]", op.ToString());
        }

        [Test]
        public void delete_with_wherecolumn()
        {
            var op = new SqlDeleteOperation { SchemaName = "dbo", TableName = "Foo" };
            op.Where("Bar", 123);
            Assert.AreEqual("DELETE FROM [dbo].[Foo] WHERE [Bar]=@Bar", op.ToString());
        }

        [Test]
        public void delete_with_whereclause()
        {
            var op = new SqlDeleteOperation { SchemaName = "dbo", TableName = "Foo" };
            op.Where("Bar=123");
            Assert.AreEqual("DELETE FROM [dbo].[Foo] WHERE Bar=123", op.ToString());
        }
    }
}