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
    public class DropConstraintTests
    {
        
        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void drop_constraint_throws_exception_without_tablename()
        {
            var op = new SqlDropConstraintOperation { SchemaName = "dbo", TableName = null, ConstraintName = "FK_Foo"};
            op.ToString();
            Assert.Fail();
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void drop_constraint_throws_exception_without_constraint()
        {
            var op = new SqlDropConstraintOperation { SchemaName = "dbo", TableName = "Foo", ConstraintName = null };
            op.ToString();
            Assert.Fail();
        }

        [Test]
        public void drop_constraint_creates_expected_sql_with_schema_and_table()
        {
            var op = new SqlDropConstraintOperation { SchemaName = "dbo", TableName = "Foo", ConstraintName = "FK_Foo"};
            Assert.AreEqual("ALTER TABLE [dbo].[Foo] DROP CONSTRAINT [FK_Foo]", op.ToString());
        }

        [Test]
        public void drop_table_creates_expected_sql_with_just_table()
        {
            var op = new SqlDropConstraintOperation { SchemaName = null, TableName = "Foo", ConstraintName = "FK_Foo" };
            Assert.AreEqual("ALTER TABLE [Foo] DROP CONSTRAINT [FK_Foo]", op.ToString());
        }
    }
}