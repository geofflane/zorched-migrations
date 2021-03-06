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
using Zorched.Migrations.Framework.Inspection;
using Zorched.Migrations.Framework.Schema;

namespace Zorched.Migrations.SqlServer.Tests.Execute
{

    [TestFixture]
    public class RenameTableTests : ExecuteBase
    {
        private const string NEW_TABLE = "MyUserTable";

        [TearDown]
        public override void Teardown()
        {
            using (var cmd = Database.CreateCommand())
            {
                cmd.CommandText = "DROP TABLE [dbo].[" + NEW_TABLE + "]";
                cmd.ExecuteNonQuery();
            }
            base.Teardown();
        }

        [Test]
        public void can_rename_table()
        {
            CreateTable(TABLE_NAME);

            Driver.Run<IRenameTableOperation>(op =>
                                                  {
                                                      op.TableName = TABLE_NAME;
                                                      op.NewTableName = NEW_TABLE;
                                                  });

            Assert.IsFalse(Driver.Inspect<ITableExistsOperation>(op => op.TableName = TABLE_NAME));
            Assert.IsTrue(Driver.Inspect<ITableExistsOperation>(op => op.TableName = NEW_TABLE));
        }
    }
}