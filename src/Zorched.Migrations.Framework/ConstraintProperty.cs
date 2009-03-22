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