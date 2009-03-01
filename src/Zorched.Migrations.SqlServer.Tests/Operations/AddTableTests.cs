using System;
using System.Data;
using NUnit.Framework;
using Zorched.Migrations.Framework;
using Zorched.Migrations.SqlServer.Schema;

namespace Zorched.Migrations.SqlServer.Tests
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