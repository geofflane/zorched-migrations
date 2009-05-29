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
using Zorched.Migrations.Framework.Data;
using Zorched.Migrations.Framework.Extensions;

namespace Zorched.Migrations.SqlServer.Data
{
    public class SqlSelectOperation : SqlBaseOperation, ISelectOperation
    {
        private readonly WhereHelper whereHelper = new WhereHelper(QUOTE_FORMAT, BaseDataOperation.PARAM_FORMAT);

        public SqlSelectOperation()
        {
            Columns = new List<string>();
        }

        public string SchemaName { get; set; }
        public string TableName { get; set; }

        public IList<string> Columns { get; protected set; }

        public void Where(params Restriction[] restrictions) { whereHelper.Where(restrictions); }
        public void Where(string rawClause) { whereHelper.Where(rawClause); }
        public void Where(string column, object val) { whereHelper.Where(column, val); }

        public IDataReader Execute(IDbCommand cmd)
        {
            cmd.CommandText = ToString();
            whereHelper.Command = cmd;
            whereHelper.AppendValues();

            return cmd.ExecuteReader();
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(TableName))
                throw new ArgumentException("TableName must be set.");

            var sb = new StringBuilder("SELECT ");
            if (0 == Columns.Count)
            {
                sb.Append("*");
            } 
            else
            {
                Columns.ForEach(c => sb.Append(c).Append(","));
                sb.TrimEnd(',');
            }
            sb.Append(" FROM ");
            
            AddTableInfo(sb, SchemaName, TableName);

            whereHelper.ClauseBuilder = sb;
            whereHelper.AppendWhere();

            return sb.ToString();
        }
    }
}