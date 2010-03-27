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

namespace Zorched.Migrations.Framework
{
    /// <summary>
    /// Represents a constraint properties.
    /// </summary>
    [Flags]
    public enum ConstraintProperty
    {
        None = 0,
        /// <summary>
        /// Cascade delete
        /// </summary>
        CascadeOnDelete = 1,
        /// <summary>
        /// Null on delete
        /// </summary>
        NullOnDelete = 2,
        /// <summary>
        /// Default on delete
        /// </summary>
        DefaultOnDelete = 4,        
        /// <summary>
        /// Cascade update
        /// </summary>
        CascadeOnUpdate = 8,
        /// <summary>
        /// Null on update
        /// </summary>
        NullOnUpdate = 16,
        /// <summary>
        /// Default on update
        /// </summary>
        DefaultOnUpdate = 32
    }

    public static class ConstraintPropertyExtensions
    {
        public static bool HasOnDelete(this ConstraintProperty prop)
        {
            return prop.Match(ConstraintProperty.CascadeOnDelete) 
                    || prop.Match(ConstraintProperty.NullOnDelete) 
                    || prop.Match(ConstraintProperty.DefaultOnDelete);
        }
        
        public static bool HasOnUpdate(this ConstraintProperty prop)
        {
            return prop.Match(ConstraintProperty.CascadeOnUpdate) 
                    || prop.Match(ConstraintProperty.NullOnUpdate) 
                    || prop.Match(ConstraintProperty.DefaultOnUpdate);
        }
      
        public static bool Match(this ConstraintProperty prop, ConstraintProperty compare)
        {
            return (prop & compare) == compare;
        }
    }
}