using System;
using NUnit.Framework;
using Zorched.Migrations.SqlServer.Schema;

namespace Zorched.Migrations.SqlServer.Tests
{
    [TestFixture]
    public class AddUniqueConstraintTests
    {
        private const string FK_WITH_SCHEMAS =
            "ALTER TABLE [dbo].[UserRoles] WITH CHECK ADD CONSTRAINT [FK_UserRoles_Roles] UNIQUE([RoleId])";

        private const string FK_NO_SCHEMAS =
            "ALTER TABLE [UserRoles] WITH CHECK ADD CONSTRAINT [FK_UserRoles_Roles] UNIQUE([RoleId])";


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
            Assert.AreEqual(FK_NO_SCHEMAS, op.ToString());
        }


        private SqlAddUniqueConstraintOperation GetFullOp()
        {
            return new SqlAddUniqueConstraintOperation
            {
                SchemaName = "dbo",
                TableName = "UserRoles",
                ColumnName = "RoleId",
                ConstraintName = "FK_UserRoles_Roles"
            };
        }
    }
}