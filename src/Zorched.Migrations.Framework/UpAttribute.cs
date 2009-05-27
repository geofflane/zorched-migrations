using System;
using System.Reflection;

namespace Zorched.Migrations.Framework
{
    /// <summary>
    /// An attribute that will mark a method in a Migration as a method
    /// to be run when Migrating to a new, larger version of the database schema.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class UpAttribute : Attribute, IMigrationDirection
    {
        public UpAttribute()
        {
            Order = 1;
        }
        
        public UpAttribute(int order)
        {
            Order = order;
        }

        public int Order { get; set; }

        public static int GetOrder(MethodInfo mi)
        {
            var upAttr = (IMigrationDirection) GetCustomAttribute(mi, typeof(UpAttribute), true);
            return upAttr.Order;
        }
    }
}