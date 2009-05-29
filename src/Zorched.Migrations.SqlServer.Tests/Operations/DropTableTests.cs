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
using NUnit.Framework;
using Zorched.Migrations.SqlServer.Schema;

namespace Zorched.Migrations.SqlServer.Tests.Operations
{
    [TestFixture]
    public class DropTableTests
    {

        [Test]
        public void drop_table_creates_expected_sql_with_schema_and_table()
        {
            var op = new SqlDropTableOperation {SchemaName = "dbo", TableName = "Foo"};
            Assert.AreEqual("DROP TABLE [dbo].[Foo]", op.ToString());
        }

        [Test]
        public void drop_table_creates_expected_sql_with_just_table()
        {
            var op = new SqlDropTableOperation { SchemaName = null, TableName = "Foo" };
            Assert.AreEqual("DROP TABLE [Foo]", op.ToString());
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void drop_table_throws_exception_without_tablename()
        {
            var op = new SqlDropTableOperation { SchemaName = "dbo", TableName = null };
            op.ToString();
            Assert.Fail();
        }
    }
}