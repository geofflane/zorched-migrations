using System;
using System.Data;
using Zorched.Migrations.Framework;

namespace Zorched.Migrations.Providers.SQLServer
{
    public class DbTypeMap
    {
        private static readonly TypeNames typeNames = new TypeNames();

        static DbTypeMap()
	    {
	        RegisterColumnType(DbType.AnsiStringFixedLength, "CHAR(255)");
            RegisterColumnType(DbType.AnsiStringFixedLength, 8000, "CHAR($l)");

            RegisterColumnType(DbType.AnsiString, "VARCHAR(MAX)");
            RegisterColumnType(DbType.AnsiString, 8000, "VARCHAR($l)");

            RegisterColumnType(DbType.Binary, "IMAGE");
            RegisterColumnType(DbType.Binary, 8000, "VARBINARY($l)");
            
            RegisterColumnType(DbType.Boolean, "BIT");
            
            RegisterColumnType(DbType.Byte, "TINYINT");
            
            RegisterColumnType(DbType.Currency, "MONEY");
            
            RegisterColumnType(DbType.Date, "DATETIME");
            RegisterColumnType(DbType.DateTime, "DATETIME");
            
            RegisterColumnType(DbType.Decimal, "DECIMAL(19,5)");
            RegisterColumnType(DbType.Decimal, 19, "DECIMAL(19, $l)");
            
            RegisterColumnType(DbType.Double, "DOUBLE PRECISION"); //synonym for FLOAT(53)
            
            RegisterColumnType(DbType.Guid, "UNIQUEIDENTIFIER");

            RegisterColumnType(DbType.Int16, "SMALLINT");
            RegisterColumnType(DbType.Int32, "INT");
            RegisterColumnType(DbType.Int64, "BIGINT");
            RegisterColumnType(DbType.Single, "REAL"); //synonym for FLOAT(24)

            RegisterColumnType(DbType.StringFixedLength, "NCHAR(255)");
            RegisterColumnType(DbType.StringFixedLength, 4000, "NCHAR($l)");

            RegisterColumnType(DbType.String, "NVARCHAR(MAX)");
            RegisterColumnType(DbType.String, 4000, "NVARCHAR($l)");

            RegisterColumnType(DbType.Time, "DATETIME");
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
            return GetTypeName(type, length, 0, 0);
        }

        /// <summary>
        /// Get the name of the database type associated with the given 
        /// </summary>
        /// <param name="type">The DbType</param>
        /// <returns>The database type name used by ddl.</returns>
        /// <param name="length"></param>
        /// <param name="precision"></param>
        /// <param name="scale"></param>
        public static string GetTypeName(DbType type, int length, int precision, int scale)
        {
            string resultWithLength = typeNames.Get(type, length, precision, scale);
            if (resultWithLength != null)
                return resultWithLength;

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