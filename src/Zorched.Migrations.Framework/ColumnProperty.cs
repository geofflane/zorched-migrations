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
    /// Represents a table column properties.
    /// </summary>
    [Flags]
    public enum ColumnProperty
    {
        None = 0,
        /// <summary>
        /// Null is allowable
        /// </summary>
        Null = 1,
        /// <summary>
        /// Null is not allowable
        /// </summary>
        NotNull = 2,
        /// <summary>
        /// Identity column, autoinc
        /// </summary>
        Identity = 4,
        /// <summary>
        /// Primary Key
        /// </summary>
        PrimaryKey = 8 | NotNull,
        /// <summary>
        /// Primary key. Make the column a PrimaryKey and unsigned
        /// </summary>
        PrimaryKeyWithIdentity = PrimaryKey | Identity
    }

    public static class ColumnPropertyExtensions
    {
        public static bool Match(this ColumnProperty prop, ColumnProperty compare)
        {
            return (prop & compare) == compare;
        }
    }
}