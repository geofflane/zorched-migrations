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
using Zorched.Migrations.Framework;
using Zorched.Migrations.SqlServer.Schema;

namespace Zorched.Migrations.SqlServer.Tests.Operations
{
    [TestFixture]
    public class ChangeColumnTests
    {
        private const string COLUMN1 = "ALTER TABLE [dbo].[Foo] ALTER COLUMN [Id] [int] NOT NULL";
        private const string COLUMN2 = "ALTER TABLE [Foo] ALTER COLUMN [Id] [int] IDENTITY(1,1) NOT NULL";

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void add_table_throws_exception_without_tablename()
        {
            var op = new SqlChangeColumnOperation { SchemaName = "dbo", TableName = null, Column = new Column()};
            op.ToString();
            Assert.Fail();
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void add_table_throws_exception_without_column()
        {
            var op = new SqlChangeColumnOperation { SchemaName = "dbo", TableName = "Foo" };
            op.ToString();
            Assert.Fail();
        }

        [Test]
        public void can_build_column1_schema()
        {
            var op = new SqlChangeColumnOperation
                         {
                             SchemaName = "dbo",
                             TableName = "Foo",
                             Column =
                                 new Column
                                     {
                                         Name = "Id",
                                         DbType = DbType.Int32,
                                         Property = ColumnProperty.NotNull
                                     }
                         };
            Assert.AreEqual(COLUMN1, op.ToString());
        }

        [Test]
        public void can_build_column2_schema()
        {
            var op = new SqlChangeColumnOperation
                         {
                             SchemaName = null,
                             TableName = "Foo",
                             Column =
                                 new Column
                                     {
                                         Name = "Id",
                                         DbType = DbType.Int32,
                                         Property = ColumnProperty.PrimaryKeyWithIdentity
                                     }
                         };
            Assert.AreEqual(COLUMN2, op.ToString());
        }
    }
}