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

using System.Data;
using Zorched.Migrations.Framework.Inspection;
using Zorched.Migrations.SqlServer.Data;

namespace Zorched.Migrations.SqlServer.Inspection
{
    public class SqlTableExistsOperation : ITableExistsOperation
    {
        private readonly SqlSelectOperation SELECT_OP = new SqlSelectOperation
                                                            {
                                                                SchemaName = "INFORMATION_SCHEMA",
                                                                TableName = "TABLES"
                                                            };
        public string SchemaName { get; set; }
        public string TableName { get; set; }

        public bool Execute(IDbCommand command)
        {
            using (var reader = SELECT_OP.Execute(command))
            {
                return reader.Read();
            }
        }

        public override string ToString()
        {
            SELECT_OP.Columns.Add("TABLE_NAME");
            SELECT_OP.Where("TABLE_NAME", TableName);
            if (!string.IsNullOrEmpty(SchemaName))
            {
                SELECT_OP.Where("TABLE_SCHEMA", SchemaName);
            }

            return SELECT_OP.ToString();
        }
    }
}