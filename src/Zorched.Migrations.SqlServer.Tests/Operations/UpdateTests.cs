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
using Zorched.Migrations.SqlServer.Data;

namespace Zorched.Migrations.SqlServer.Tests.Operations
{
    [TestFixture]
    public class UpdateTests
    {
        [Test]
        public void update_with_schema()
        {
            var op = new SqlUpdateOperation { SchemaName = "dbo", TableName = "Foo" };
            op.Columns.Add("var");
            op.Columns.Add("var2");
            Assert.AreEqual("UPDATE [dbo].[Foo] SET [var]=@var,[var2]=@var2", op.ToString());
        }

        [Test]
        public void update_no_schema()
        {
            var op = new SqlUpdateOperation{ SchemaName = null, TableName = "Foo" };
            op.Columns.Add("var");
            op.Columns.Add("var2");
            Assert.AreEqual("UPDATE [Foo] SET [var]=@var,[var2]=@var2", op.ToString());
        }

        [Test]
        public void update_with_where_clause()
        {
            var op = new SqlUpdateOperation { SchemaName = "dbo", TableName = "Foo" };
            op.Columns.Add("var");
            op.Columns.Add("var2");
            op.Where("bar", "baz");

            Assert.AreEqual("UPDATE [dbo].[Foo] SET [var]=@var,[var2]=@var2 WHERE [bar]=@bar", op.ToString());
        }
    }
}