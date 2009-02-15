using System;

namespace Zorched.Migrations.Framework
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Assembly)]
    public class DriverAttribute : Attribute
    {
        public DriverAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; set; }


        public static string GetDriverName(Type t)
        {
            var upAttr = (DriverAttribute)GetCustomAttribute(t, typeof(DriverAttribute), true);
            return upAttr.Name;
        }
    }
}