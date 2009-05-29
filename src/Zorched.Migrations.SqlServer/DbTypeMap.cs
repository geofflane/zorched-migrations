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
using Zorched.Migrations.Framework;

namespace Zorched.Migrations.SqlServer
{
    public class DbTypeMap
    {
        private static readonly TypeNames typeNames = new TypeNames();

        static DbTypeMap()
	    {
	        RegisterColumnType(DbType.AnsiStringFixedLength, "[char](255)");
            RegisterColumnType(DbType.AnsiStringFixedLength, 8000, "[char]($l)");

            RegisterColumnType(DbType.AnsiString, "[varchar](MAX)");
            RegisterColumnType(DbType.AnsiString, 8000, "[varchar]($l)");

            RegisterColumnType(DbType.Binary, "[image]");
            RegisterColumnType(DbType.Binary, 8000, "[varbinary]($l)");
            
            RegisterColumnType(DbType.Boolean, "[bit]");
            
            RegisterColumnType(DbType.Byte, "[tinyint]");
            
            RegisterColumnType(DbType.Currency, "[money]");
            
            RegisterColumnType(DbType.Date, "[datetime]");
            RegisterColumnType(DbType.DateTime, "[datetime]");
            
            RegisterColumnType(DbType.Decimal, "[decimal](19,5)");
            RegisterColumnType(DbType.Decimal, 19, "[decimal](19, $l)");
            
            RegisterColumnType(DbType.Double, "[double precision]"); //synonym for FLOAT(53)
            
            RegisterColumnType(DbType.Guid, "[uniqueidentifier]");

            RegisterColumnType(DbType.Int16, "[smallint]");
            RegisterColumnType(DbType.Int32, "[int]");
            RegisterColumnType(DbType.Int64, "[bigint]");
            RegisterColumnType(DbType.Single, "[real]"); //synonym for FLOAT(24)

            RegisterColumnType(DbType.StringFixedLength, "[nchar](255)");
            RegisterColumnType(DbType.StringFixedLength, 4000, "[nchar]($l)");

            RegisterColumnType(DbType.String, "[nvarchar](MAX)");
            RegisterColumnType(DbType.String, 4000, "[nvarchar]($l)");

            RegisterColumnType(DbType.Time, "[datetime]");

            RegisterColumnType(DbType.Xml, "[xml](255)");
            RegisterColumnType(DbType.Xml, 8000, "[xml]($l)");

        }

        /// <summary>
        /// Subclasses register a typename for the given type code and maximum
        /// column length. <c>$l</c> in the type name will be replaced by the column
        /// length (if appropriate)
        /// </summary>
        /// <param name="code">The typecode</param>
        /// <param name="capacity">Maximum length of database type</param>
        /// <param name="name">The database type name</param>
        protected static void RegisterColumnType(DbType code, int capacity, string name)
        {
            typeNames.Put(code, capacity, name);
        }

        /// <summary>
        /// Suclasses register a typename for the given type code. <c>$l</c> in the 
        /// typename will be replaced by the column length (if appropriate).
        /// </summary>
        /// <param name="code">The typecode</param>
        /// <param name="name">The database type name</param>
        protected static void RegisterColumnType(DbType code, string name)
        {
            typeNames.Put(code, name);
        }

        /// <summary>
        /// Get the name of the database type associated with the given 
        /// </summary>
        /// <param name="type">The DbType</param>
        /// <returns>The database type name used by ddl.</returns>
        /// <param name="length"></param>
        public static string GetTypeName(DbType type, int length)
        {
            if (length > 0)
            {
                string resultWithLength = typeNames.Get(type, length, 0, 0);
                if (resultWithLength != null)
                    return resultWithLength;
            }

            return GetTypeName(type);
        }

        /// <summary>
        /// Get the name of the database type associated with the given 
        /// </summary>
        /// <param name="type">The DbType</param>
        /// <returns>The database type name used by ddl.</returns>
        public static string GetTypeName(DbType type)
        {
            string result = typeNames.Get(type);
            if (result == null)
            {
                throw new Exception(string.Format("No default type mapping for DbType {0}", type));
            }

            return result;
        }
    }
}