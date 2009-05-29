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

using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Zorched.Migrations.Framework;
using Zorched.Migrations.Framework.Extensions;
using Zorched.Migrations.Framework.Schema;

namespace Zorched.Migrations.SqlServer.Schema
{
    public class SqlAddTableOperation : BaseSchemaOperation, IAddTableOperation
    {

        private const string PK_FORMAT = QUOTE_FORMAT + " ASC";
        private const string PK_BLOCK =
@"PRIMARY KEY CLUSTERED 
(
{0}
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)";

        public SqlAddTableOperation()
        {
            SchemaName = "dbo";
            Columns = new List<Column>();
        }

        public IList<Column> Columns { get; private set; }

        public void AddColumn(Column c)
        {
            Columns.Add(c);
        }

        public void AddColumn(string name, DbType type, ColumnProperty property, object defaultValue)
        {
            AddColumn(new Column {Name=name, DbType = type, Property = property, DefaultValue = defaultValue});
        }

        public void AddColumn(string name, DbType type, int size, object defaultValue)
        {
            AddColumn(new Column {Name = name, DbType = type, Size = size, DefaultValue = defaultValue});
        }

        public void AddColumn(string name, DbType type)
        {
            AddColumn(new Column { Name = name, DbType = type });
        }

        public void AddColumn(string name, DbType type, ColumnProperty prop)
        {
            AddColumn(new Column { Name = name, DbType = type, Property = prop });
        }

        public void AddColumn(string name, DbType type, int size)
        {
            AddColumn(new Column { Name = name, DbType = type, Size = size });
        }

        public void AddColumn(string name, DbType type, int size, ColumnProperty prop)
        {
            AddColumn(new Column { Name = name, DbType = type, Size = size, Property = prop });
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(TableName))
                throw new ArgumentException("TableName must be set.");

            var sb = new StringBuilder("CREATE TABLE ");
            AddTableInfo(sb, SchemaName, TableName);

            sb.AppendLine("(");
            AddColumns(sb);
            sb.Append(")");

            return sb.ToString();
        }

        private void AddColumns(StringBuilder sb)
        {
            var pks = new List<Column>();

            Columns.ForEach(
                c =>
                    {
                        AddColumnDefinition(sb, c);
                        sb.AppendLine(",");

                        if (c.Property.Match(ColumnProperty.PrimaryKey))
                        {
                            pks.Add(c);
                        }
                    });

            if (0 != pks.Count)
            {
                var pksSb = new StringBuilder();
                pks.ForEach(c => pksSb.AppendFormat(PK_FORMAT, c.Name).AppendLine(","));
                pksSb.TrimEnd().TrimEnd(',');
                sb.AppendFormat(PK_BLOCK, pksSb).AppendLine();
            }

            sb.TrimEnd().TrimEnd(',');
        }
    }
}