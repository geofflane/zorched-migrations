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
using System.Data;
using System.Text;
using Zorched.Migrations.Framework;
using Zorched.Migrations.Framework.Data;

namespace Zorched.Migrations.SqlServer.Data
{
    public class SqlDeleteOperation : BaseDataOperation, IDeleteOperation
    {
        private readonly WhereHelper whereHelper = new WhereHelper(QUOTE_FORMAT, PARAM_FORMAT);

        public void Where(params Restriction[] restrictions) { whereHelper.Where(restrictions); }
        public void Where(string rawClause) { whereHelper.Where(rawClause); }
        public void Where(string column, object val) { whereHelper.Where(column, val); }

        public void Execute(IDbCommand command)
        {
            command.CommandText = ToString();
            whereHelper.Command = command;
            whereHelper.AppendValues();
            command.ExecuteNonQuery();
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(TableName))
                throw new ArgumentException("TableName must be set.");

            var sb = new StringBuilder("DELETE FROM ");
            AddTableInfo(sb, SchemaName, TableName);

            whereHelper.ClauseBuilder = sb;
            whereHelper.AppendWhere();

            return sb.ToString();
        }
    }
}