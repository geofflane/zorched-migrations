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
using System.Text;

namespace Zorched.Migrations.Framework
{
    /// <summary>
    /// The interface that needs to be implemented by classes that want to
    /// interact with Restrictions.
    /// </summary>
    public interface IRestrictionVisitor
    {
        StringBuilder ClauseBuilder { get; set; }

        IDbCommand Command { get;}

        void AddClause(string clause);

        void AddParameter(string name);

        void AddValue(string name, object value);

    }

    /// <summary>
    /// Defines the ways that parameters in queries can be combined.
    /// </summary>
    public abstract class Restriction
    {

        public abstract void AddParams(IRestrictionVisitor visitor);

        public abstract void AddValues(IRestrictionVisitor visitor);

        public static CustomRestriction Column(string clause)
        {
            return new CustomRestriction(clause);
        }

        public static EqualsRestriction Equals(string name, object value)
        {
            return new EqualsRestriction(name, value);
        }

        public static OrRestriction Or(params Restriction[] restrictions)
        {
            return new OrRestriction(restrictions);
        }

        public static AndRestriction And(params Restriction[] restrictions)
        {
            return new AndRestriction(restrictions);
        }
    }

    public class CustomRestriction : Restriction
    {
        public CustomRestriction(string clause)
        {
            Clause = clause;
        }

        public string Clause { get; set; }

        public override void AddParams(IRestrictionVisitor visitor)
        {
            visitor.AddClause(Clause);
        }

        public override void AddValues(IRestrictionVisitor visitor)
        {
            // no op
        }
    }

    public class EqualsRestriction : Restriction
    {
        public EqualsRestriction(string name, object value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; set; }
        public object Value { get; set; }


        public override void AddParams(IRestrictionVisitor visitor)
        {
            visitor.AddParameter(Name);
        }

        public override void AddValues(IRestrictionVisitor visitor)
        {
            visitor.AddValue(Name, Value);
        }
    }

    public abstract class AggregateRestriction : Restriction
    {
        public Restriction[] Restrictions { get; set; }
        public abstract string Join { get; }

        protected void Open(IRestrictionVisitor visitor)
        {
            if (Restrictions.Length > 1)
            {
                visitor.ClauseBuilder.Append("(");
            }
        }

        protected void Close(IRestrictionVisitor visitor)
        {
            if (Restrictions.Length > 1)
            {
                visitor.ClauseBuilder.Append(")");
            }
        }

        public override void AddParams(IRestrictionVisitor visitor)
        {
            Open(visitor);
            for (int i = 0; i < Restrictions.Length; i++)
            {
                Restrictions[i].AddParams(visitor);
                if (i < Restrictions.Length - 1)
                {
                    visitor.ClauseBuilder.Append(Join);
                }
            }
            Close(visitor);
        }

        public override void AddValues(IRestrictionVisitor visitor)
        {
            foreach (var r in Restrictions)
            {
                r.AddValues(visitor);
            }
        }
    }

    public class OrRestriction : AggregateRestriction
    {
        public OrRestriction(params Restriction[] restrictions)
        {
            Restrictions = restrictions;
        }
        public override string Join { get { return " OR"; } }
    }

    public class AndRestriction : AggregateRestriction
    {
        public AndRestriction(params Restriction[] restrictions)
        {
            Restrictions = restrictions;
        }

        public override string Join { get { return " AND"; } }
    }
}