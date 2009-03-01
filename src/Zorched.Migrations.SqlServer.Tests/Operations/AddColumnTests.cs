using System;
using System.Data;
using NUnit.Framework;
using Zorched.Migrations.Framework;
using Zorched.Migrations.SqlServer.Schema;

namespace Zorched.Migrations.SqlServer.Tests.Operations
{
    [TestFixture]
    public class AddColumnTests
    {
        private const string COLUMN1 = "ALTER TABLE [dbo].[Foo] ADD [Id] [int] NOT NULL";
        private const string COLUMN2 = "ALTER TABLE [Foo] ADD [Id] [int] IDENTITY(1,1) NOT NULL";

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void add_table_throws_exception_without_tablename()
        {
            var op = new SqlAddColumnOperation { SchemaName = "dbo", TableName = null, Column = new Column()};
            op.ToString();
            Assert.Fail();
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void add_table_throws_exception_without_column()
        {
            var op = new SqlAddColumnOperation { SchemaName = "dbo", TableName = "Foo"};
            op.ToString();
            Assert.Fail();
        }

        [Test]
        public void can_build_column1_schema()
        {
            var op = new SqlAddColumnOperation
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
            var op = new SqlAddColumnOperation
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