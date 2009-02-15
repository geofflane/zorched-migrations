using System;
using System.Reflection;

namespace Zorched.Migrations.Framework
{
    [AttributeUsage(AttributeTargets.Method)]
    public class DownAttribute : Attribute, IMigrationDirection
    {
        public DownAttribute()
        {
            Order = 1;
        }
        
        public DownAttribute(int order)
        {
            Order = order;
        }

        public int Order { get; set; }

        public static int GetOrder(MethodInfo mi)
        {
            var upAttr = (IMigrationDirection) GetCustomAttribute(mi, typeof(DownAttribute), true);
            return upAttr.Order;
        }
    }
}