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