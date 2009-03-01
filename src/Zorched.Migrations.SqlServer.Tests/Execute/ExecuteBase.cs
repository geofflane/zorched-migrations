using System.Configuration;
using System.Data;
using System.Data.Common;
using NUnit.Framework;
using Zorched.Migrations.Core;
using Zorched.Migrations.Framework;

namespace Zorched.Migrations.SqlServer.Tests.Execute
{

    [TestFixture]
    public class ExecuteBase
    {
        protected const string TABLE_NAME = "User";

        protected SqlServerDriver Driver;
        protected DbParams Database;

        [SetUp]
        public virtual void Setup()
        {
            var cs = ConfigurationManager.ConnectionStrings["SqlServer"];
            var factory = DbProviderFactories.GetFactory(cs.ProviderName);
            var connection = factory.CreateConnection();
            connection.ConnectionString = cs.ConnectionString;

            Database = new DbParams(connection);
            Database.BeginTransaction();

            Driver = new SqlServerDriver(Database, new TestLogger());
        }

        [TearDown]
        public virtual void Teardown()
        {
            Database.Transaction.Rollback();
            Database.Connection.Close();
        }

        protected void CreateTable(string tableName)
        {
            Driver.AddTable(op =>
            {
                op.TableName = tableName;
                op.Columns.Add(new Column { Name = "Id", DbType = DbType.Int32, Property = ColumnProperty.PrimaryKeyWithIdentity });
                op.Columns.Add(new Column { Name = "Username", DbType = DbType.String, Property = ColumnProperty.NotNull, Size = 50 });
                op.Columns.Add(new Column { Name = "Password", DbType = DbType.String, Property = ColumnProperty.NotNull, Size = 50 });
                op.Columns.Add(new Column { Name = "Email", DbType = DbType.String, Property = ColumnProperty.Null, Size = 50 });
            });
        }
    }
}