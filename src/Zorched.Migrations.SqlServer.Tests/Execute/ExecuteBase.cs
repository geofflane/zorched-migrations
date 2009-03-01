using System.Configuration;
using System.Data;
using System.Data.Common;
using NUnit.Framework;
using Zorched.Migrations.Framework;

namespace Zorched.Migrations.SqlServer.Tests.Execute
{

    [TestFixture]
    public class ExecuteBase
    {
        protected const string TABLE_NAME = "User";

        protected SqlServerDriver Driver;
        protected DbConnection Connection;
//        protected DbTransaction Transaction;

        [SetUp]
        public virtual void Setup()
        {
            var cs = ConfigurationManager.ConnectionStrings["SqlServer"];
            var factory = DbProviderFactories.GetFactory(cs.ProviderName);
            Connection = factory.CreateConnection();
            Connection.ConnectionString = cs.ConnectionString;
            Connection.Open();
            Driver = new SqlServerDriver(Connection, new TestLogger());

//            Transaction = Connection.BeginTransaction();
        }

        [TearDown]
        public virtual void Teardown()
        {
//            Transaction.Rollback();
            Connection.Close();
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