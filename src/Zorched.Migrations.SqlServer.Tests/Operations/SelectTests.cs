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
    public class SelectTests
    {
        [Test]
        public void select_generates_proper_form_without_columns()
        {
            var op = new SqlSelectOperation {SchemaName = "dbo", TableName = "Foo"};
            Assert.AreEqual("SELECT * FROM [dbo].[Foo]", op.ToString());
        }

        [Test]
        public void select_generates_proper_form_without_columns_no_schema()
        {
            var op = new SqlSelectOperation { SchemaName = null, TableName = "Foo" };
            Assert.AreEqual("SELECT * FROM [Foo]", op.ToString());
        }

        [Test]
        public void select_generates_proper_form_with_columns()
        {
            var op = new SqlSelectOperation { SchemaName = "dbo", TableName = "Foo" };
            op.Columns.Add("Id");
            op.Columns.Add("Bar");
            Assert.AreEqual("SELECT Id,Bar FROM [dbo].[Foo]", op.ToString());
        }

        [Test]
        public void select_generates_proper_form_with_columns_and_where_column()
        {
            var op = new SqlSelectOperation { SchemaName = "dbo", TableName = "Foo" };
            op.Columns.Add("Id");
            op.Columns.Add("Bar");
            op.Where("Baz","123");

            Assert.AreEqual("SELECT Id,Bar FROM [dbo].[Foo] WHERE [Baz]=@Baz", op.ToString());
        }

        [Test]
        public void select_generates_proper_form_with_columns_and_where_clause()
        {
            var op = new SqlSelectOperation { SchemaName = "dbo", TableName = "Foo" };
            op.Columns.Add("Id");
            op.Columns.Add("Bar");
            op.Where("Baz=123");

            Assert.AreEqual("SELECT Id,Bar FROM [dbo].[Foo] WHERE Baz=123", op.ToString());
        }
    }
}