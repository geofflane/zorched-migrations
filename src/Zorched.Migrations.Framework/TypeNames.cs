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
using Zorched.Migrations.Framework.Extensions;

namespace Zorched.Migrations.Framework
{
    /// <summary>
    /// This class maps a DbType to names.
    /// </summary>
    /// <remarks>
    /// Associations may be marked with a capacity. Calling the <c>Get()</c>
    /// method with a type and actual size n will return the associated
    /// name with smallest capacity >= n, if available and an unmarked
    /// default type otherwise.
    /// Eg, setting
    /// <code>
    ///		Names.Put(DbType,			"TEXT" );
    ///		Names.Put(DbType,	255,	"VARCHAR($l)" );
    ///		Names.Put(DbType,	65534,	"LONGVARCHAR($l)" );
    /// </code>
    /// will give you back the following:
    /// <code>
    ///		Names.Get(DbType)			// --> "TEXT" (default)
    ///		Names.Get(DbType,100)		// --> "VARCHAR(100)" (100 is in [0:255])
    ///		Names.Get(DbType,1000)	// --> "LONGVARCHAR(1000)" (100 is in [256:65534])
    ///		Names.Get(DbType,100000)	// --> "TEXT" (default)
    /// </code>
    /// On the other hand, simply putting
    /// <code>
    ///		Names.Put(DbType, "VARCHAR($l)" );
    /// </code>
    /// would result in
    /// <code>
    ///		Names.Get(DbType)			// --> "VARCHAR($l)" (will cause trouble)
    ///		Names.Get(DbType,100)		// --> "VARCHAR(100)" 
    ///		Names.Get(DbType,1000)	// --> "VARCHAR(1000)"
    ///		Names.Get(DbType,10000)	// --> "VARCHAR(10000)"
    /// </code>
    /// </remarks>
    public class TypeNames
    {
        public const string LengthPlaceHolder = "$l";
        public const string PrecisionPlaceHolder = "$p";
        public const string ScalePlaceHolder = "$s";

        private readonly Dictionary<DbType, SortedList<int, string>> weighted = new Dictionary<DbType, SortedList<int, string>>();

        private readonly Dictionary<DbType, string> defaults = new Dictionary<DbType, string>();

        /// <summary>
        /// Get default type name for specified type
        /// </summary>
        /// <param name="typecode">the type key</param>
        /// <returns>the default type name associated with the specified key</returns>
        public string Get(DbType typecode)
        {
            string result;
            if (!defaults.TryGetValue(typecode, out result))
            {
                throw new ArgumentException("Dialect does not support DbType." + typecode, "typecode");
            }
            return result;
        }

        /// <summary>
        /// Get the type name specified type and size
        /// </summary>
        /// <param name="typecode">the type key</param>
        /// <param name="size">the SQL length </param>
        /// <param name="scale">the SQL scale </param>
        /// <param name="precision">the SQL precision </param>
        /// <returns>
        /// The associated name with smallest capacity >= size if available and the
        /// default type name otherwise
        /// </returns>
        public string Get(DbType typecode, int size, int precision, int scale)
        {
            SortedList<int, string> map;
            weighted.TryGetValue(typecode, out map);
            if (map != null && map.Count > 0)
            {
                foreach (KeyValuePair<int, string> entry in map)
                {
                    if (size <= entry.Key)
                    {
                        return Replace(entry.Value, size, precision, scale);
                    }
                }
            }
            //Could not find a specific type for the size, using the default
            return Replace(Get(typecode), size, precision, scale);
        }

        private static string Replace(string type, int size, int precision, int scale)
        {
            type = type.ReplaceOnce(LengthPlaceHolder, size.ToString());
            type = type.ReplaceOnce(ScalePlaceHolder, scale.ToString());
            return type.ReplaceOnce(PrecisionPlaceHolder, precision.ToString());
        }

        /// <summary>
        /// Set a type name for specified type key and capacity
        /// </summary>
        /// <param name="typecode">the type key</param>
        /// <param name="capacity">the (maximum) type size/length</param>
        /// <param name="value">The associated name</param>
        public void Put(DbType typecode, int capacity, string value)
        {
            SortedList<int, string> map;
            if (!weighted.TryGetValue(typecode, out map))
            {
                // add new ordered map
                weighted[typecode] = map = new SortedList<int, string>();
            }
            map[capacity] = value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="typecode"></param>
        /// <param name="value"></param>
        public void Put(DbType typecode, string value)
        {
            defaults[typecode] = value;
        }
    }
}