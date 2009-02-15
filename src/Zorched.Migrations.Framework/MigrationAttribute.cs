using System;
using System.Collections.Generic;
using System.Reflection;
using Zorched.Migrations.Framework.Extensions;

namespace Zorched.Migrations.Framework
{
    [AttributeUsage(AttributeTargets.Class)]
    public class MigrationAttribute : Attribute
    {
        public long Version { get; set; }

        public MigrationAttribute(long ver)
        {
            Version = ver;
        }

        public static long GetVersion(Type t)
        {
            var migAttr = (MigrationAttribute) GetCustomAttribute(t, typeof(MigrationAttribute), true);
            return migAttr.Version;
        }

        public static IEnumerable<Type> GetTypes(Assembly assembly)
        {
            return assembly.GetTypesWithAttribute(typeof(MigrationAttribute));
        }
    }
}