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

namespace Zorched.Migrations.SqlServer.Data
{
    public class WhereHelper : IRestrictionVisitor
    {
        private readonly List<Restriction> restrictions = new List<Restriction>();

        public WhereHelper(string quoteFormat, string paramFormat)
        {
            ClauseBuilder = new StringBuilder();
            ParamFormat = paramFormat;
            QuoteFormat = quoteFormat;
        }

        public string QuoteFormat { get; set; }
        public string ParamFormat { get; set; }
        public StringBuilder ClauseBuilder { get; set; }
        public IDbCommand Command { get; set; }

        public void Where(params Restriction[] rests)
        {
            if (rests.Length == 1 && restrictions.Count != 0)
            {
                var lastR = restrictions[restrictions.Count - 1];
                restrictions.RemoveAt(restrictions.Count - 1);
                restrictions.Add(new AndRestriction(lastR, rests[0]));
            }
            else if(rests.Length == 1)
            {
                restrictions.Add(rests[0]);
            }
            else
            {
                restrictions.Add(new AndRestriction(rests));
            }
        }

        public void Where(string rawClause)
        {
            restrictions.Add(new CustomRestriction(rawClause));
        }

        public void Where(string column, object val)
        {
            Where(new EqualsRestriction(column, val));
        }

        public bool NeedsWhereClause
        {
            get { return restrictions.Count > 0; }
        }

        public void AppendWhere()
        {
            if (NeedsWhereClause)
            {
                ClauseBuilder.Append(" WHERE");
                restrictions.ForEach(r => r.AddParams(this));
            }
        }

        public void AppendValues()
        {
            if (NeedsWhereClause)
            {
                restrictions.ForEach(r => r.AddValues(this));
            }
        }

        public void AddClause(string clause)
        {
            AddSpace().Append(clause);
        }

        public void AddParameter(string name)
        {
            AddSpace().AppendFormat(QuoteFormat, name).Append("=").AppendFormat(ParamFormat, name);
        }

        public void AddValue(string name, object value)
        {
            var whereParam = Command.CreateParameter();
            whereParam.ParameterName = String.Format(ParamFormat, name);
            whereParam.Value = value;
            Command.Parameters.Add(whereParam);
        }
        
        private StringBuilder AddSpace()
        {
            return ClauseBuilder.Append(" ");
        }
    }
}