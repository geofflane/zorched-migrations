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
    public class AddTableTests
    {

        private const string TABLE1 =
            @"CREATE TABLE [dbo].[Foo](
[Id] [int] IDENTITY(1,1) NOT NULL,
[BarId] [int] NOT NULL,
[Name] [nvarchar](50) NOT NULL,
[Description] [nvarchar](MAX) NULL)";

        private const string TABLE2 =
            @"CREATE TABLE [dbo].[Foo](
[Id] [int] IDENTITY(1,1) NOT NULL,
[BarId] [int] NOT NULL,
[Name] [nvarchar](50) NOT NULL,
[Description] [nvarchar](MAX) NULL,
PRIMARY KEY CLUSTERED 
(
[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON))";

        private const string TABLE3 =
            @"CREATE TABLE [dbo].[Foo](
[Id] [int] IDENTITY(1,1) NOT NULL,
[Id2] [int] IDENTITY(1,1) NOT NULL,
[BarId] [int] NOT NULL,
[Name] [nvarchar](50) NOT NULL,
[Description] [nvarchar](MAX) NULL,
PRIMARY KEY CLUSTERED 
(
[Id] ASC,
[Id2] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON))";

        [Test]
        public void can_build_table1_schema()
        {
            var op = new SqlAddTableOperation { SchemaName = "dbo", TableName = "Foo" };
            op.AddColumn("Id", DbType.Int32, ColumnProperty.Identity);
            op.AddColumn("BarId", DbType.Int32, ColumnProperty.NotNull);
            op.AddColumn("Name", DbType.String, 50, ColumnProperty.NotNull);
            op.AddColumn("Description", DbType.String, ColumnProperty.Null);
            Assert.AreEqual(TABLE1, op.ToString());
        }

        [Test]
        public void can_build_table2_schema()
        {
            var op = new SqlAddTableOperation { SchemaName = "dbo", TableName = "Foo" };
            op.AddColumn("Id", DbType.Int32, ColumnProperty.PrimaryKeyWithIdentity);
            op.AddColumn("BarId", DbType.Int32, ColumnProperty.NotNull);
            op.AddColumn("Name", DbType.String, 50, ColumnProperty.NotNull);
            op.AddColumn("Description", DbType.String, ColumnProperty.Null);
            Assert.AreEqual(TABLE2, op.ToString());
        }

        [Test]
        public void can_build_table3_schema()
        {
            var op = new SqlAddTableOperation { SchemaName = "dbo", TableName = "Foo" };
            op.AddColumn("Id", DbType.Int32, ColumnProperty.PrimaryKeyWithIdentity);
            op.AddColumn("Id2", DbType.Int32, ColumnProperty.PrimaryKeyWithIdentity);
            op.AddColumn("BarId", DbType.Int32, ColumnProperty.NotNull);
            op.AddColumn("Name", DbType.String, 50, ColumnProperty.NotNull);
            op.AddColumn("Description", DbType.String, ColumnProperty.Null);
            Assert.AreEqual(TABLE3, op.ToString());
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void add_table_throws_exception_without_tablename()
        {
            var op = new SqlAddTableOperation { SchemaName = "dbo", TableName = null };
            op.ToString();
            Assert.Fail();
        }
    }
}