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