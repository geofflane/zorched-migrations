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
    public class AddForeignKeyTests
    {
        private const string FK_WITH_SCHEMAS =
            "ALTER TABLE [dbo].[UserRoles] WITH CHECK ADD CONSTRAINT [FK_UserRoles_Roles] FOREIGN KEY([RoleId]) REFERENCES [dbo].[Role] ([Id])";

        private const string FK_NO_SCHEMAS =
            "ALTER TABLE [UserRoles] WITH CHECK ADD CONSTRAINT [FK_UserRoles_Roles] FOREIGN KEY([RoleId]) REFERENCES [Role] ([Id])";


        [Test]
        [ExpectedException(typeof (ArgumentException))]
        public void add_fk_throws_exception_without_tablename()
        {
            var op = GetFullOp();
            op.TableName = null;
            op.ToString();
            Assert.Fail();
        }

        [Test]
        [ExpectedException(typeof (ArgumentException))]
        public void add_fk_throws_exception_without_constraint()
        {
            var op = GetFullOp();
            op.ConstraintName = null;
            op.ToString();
            Assert.Fail();
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void add_fk_throws_exception_without_columnname()
        {
            var op = GetFullOp();
            op.ColumnName = null;
            op.ToString();
            Assert.Fail();
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void add_fk_throws_exception_without_referencetablename()
        {
            var op = GetFullOp();
            op.ReferenceTableName = null;
            op.ToString();
            Assert.Fail();
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void add_fk_throws_exception_without_referencecolumnname()
        {
            var op = GetFullOp();
            op.ReferenceColumnName = null;
            op.ToString();
            Assert.Fail();
        }

        [Test]
        public void add_fk_creates_expected_sql_with_schema_and_table()
        {
            var op = GetFullOp();
            Assert.AreEqual(FK_WITH_SCHEMAS, op.ToString());
        }

        [Test]
        public void add_fk_creates_expected_sql_with_just_table()
        {
            var op = GetFullOp();
            op.SchemaName = null;
            op.ReferenceSchemaName = null;
            Assert.AreEqual(FK_NO_SCHEMAS, op.ToString());
        }


        private SqlAddForeignKeyOperation GetFullOp()
        {
            return new SqlAddForeignKeyOperation
                       {
                           SchemaName = "dbo",
                           TableName = "UserRoles",
                           ColumnName = "RoleId",
                           ConstraintName = "FK_UserRoles_Roles",
                           ReferenceSchemaName = "dbo",
                           ReferenceTableName = "Role",
                           ReferenceColumnName = "Id"
                       };
        }
    }
}