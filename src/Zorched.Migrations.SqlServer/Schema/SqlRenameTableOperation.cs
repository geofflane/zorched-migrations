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

using System.Text;
using Zorched.Migrations.Framework.Schema;

namespace Zorched.Migrations.SqlServer.Schema
{
    public class SqlRenameTableOperation : BaseSchemaOperation, IRenameTableOperation
    {
        private const string QUOTE_VALUE = "'{0}'";

        public string NewTableName { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder("EXEC sp_rename ");
            sb.Append("'");
            if (! string.IsNullOrEmpty(SchemaName))
            {
                sb.Append(SchemaName).Append(".");
            }
            sb.Append(TableName);
            sb.Append("', ").AppendFormat(QUOTE_VALUE, NewTableName);

            return sb.ToString();
        }
    }
}