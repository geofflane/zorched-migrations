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