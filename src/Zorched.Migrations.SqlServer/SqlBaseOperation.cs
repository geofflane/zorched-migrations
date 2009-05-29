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
using Zorched.Migrations.Framework;

namespace Zorched.Migrations.SqlServer
{
    public abstract class SqlBaseOperation
    {
        protected const string QUOTE_FORMAT = "[{0}]";

        public void AddColumnDefinition(StringBuilder sb, Column c)
        {
            sb.AppendFormat(QUOTE_FORMAT, c.Name);
            sb.Append(" ").Append(DbTypeMap.GetTypeName(c.DbType, c.Size));
            if (c.Property.Match(ColumnProperty.Identity))
            {
                sb.Append(" IDENTITY(1,1)");
            }
            sb.Append(c.Property.Match(ColumnProperty.Null) ? " NULL" : " NOT NULL");
            if (null != c.DefaultValue)
            {
                sb.Append(" DEFAULT ").Append(c.DefaultValue);
            }
        }

        public void AddTableInfo(StringBuilder sb, string schemaName, string tableName)
        {
            if (!string.IsNullOrEmpty(schemaName))
            {
                sb.AppendFormat(QUOTE_FORMAT, schemaName).Append(".");
            }
            sb.AppendFormat(QUOTE_FORMAT, tableName);
        }
    }
}