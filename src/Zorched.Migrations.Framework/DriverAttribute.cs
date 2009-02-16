using System;
using System.Reflection;
using Zorched.Migrations.Framework.Extensions;

namespace Zorched.Migrations.Framework
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DriverAttribute : Attribute
    {
        public DriverAttribute(string name, string provider)
        {
            Name = name;
            Provider = provider;
        }

        public string Name { get; set; }
        public string Provider { get; set; }

        public static Type GetDriver(Assembly assembly)
        {
            var types = assembly.GetTypesWithAttribute(typeof (DriverAttribute));
            var enumerator = types.GetEnumerator();
            if (! enumerator.MoveNext())
                throw new Exception("No driver found.");

            return enumerator.Current;
        }

        public static string GetDriverName(Type t)
        {
            var upAttr = (DriverAttribute)GetCustomAttribute(t, typeof(DriverAttribute), true);
            return upAttr.Name;
        }

        public static string GetProvider(Type t)
        {
            var upAttr = (DriverAttribute)GetCustomAttribute(t, typeof(DriverAttribute), true);
            return upAttr.Provider;
        }
    }
}