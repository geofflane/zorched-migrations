using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Zorched.Migrations.Framework
{
    /// <summary>
    /// This attribute will mark a Migration method as something that should be run
    /// only when the currently running Driver type matches one of the Drivers in 
    /// the list supplied.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class OnlyWhenDriverAttribute : Attribute
    {
        public OnlyWhenDriverAttribute(string drivers)
        {
            Drivers = new List<string>(drivers.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
        }

        public IList<string> Drivers { get; set; }

        public static bool ShouldRun(MethodInfo mi, string driverName)
        {
            if (! HasAttribute(mi))
                return true;

            return IncludesDriver(mi, driverName);
        }

        public static bool HasAttribute(MethodInfo mi)
        {
            return null != GetAttribute(mi);
        }

        public static IList<string> GetDriverNames(MethodInfo mi)
        {
            return GetAttribute(mi).Drivers;
        }

        public static bool IncludesDriver(MethodInfo mi, string driverName)
        {
            return GetAttribute(mi).Drivers.Any(d => d == driverName);
        }

        private static OnlyWhenDriverAttribute GetAttribute(MemberInfo mi)
        {
            return (OnlyWhenDriverAttribute)GetCustomAttribute(mi, typeof(OnlyWhenDriverAttribute), true);   
        }
    }
}