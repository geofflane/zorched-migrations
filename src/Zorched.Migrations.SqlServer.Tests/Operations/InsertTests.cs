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
    public class InsertTests
    {
        [Test]
        public void insert_with_schema()
        {
            var op = new SqlInsertOperation() { SchemaName = "dbo", TableName = "Foo" };
            op.Columns.Add("var");
            op.Columns.Add("var2");
            Assert.AreEqual("INSERT INTO [dbo].[Foo] ([var],[var2]) VALUES (@var,@var2)", op.ToString());
        }

        [Test]
        public void insert_no_schema()
        {
            var op = new SqlInsertOperation { SchemaName = null, TableName = "Foo" };
            op.Columns.Add("var");
            op.Columns.Add("var2");
            Assert.AreEqual("INSERT INTO [Foo] ([var],[var2]) VALUES (@var,@var2)", op.ToString());
        }
    }
}