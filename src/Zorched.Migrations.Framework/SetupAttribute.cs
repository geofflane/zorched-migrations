using System;
using System.Reflection;
using Zorched.Migrations.Framework.Extensions;

namespace Zorched.Migrations.Framework
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class SetupAttribute : Attribute
    {
        public static Type GetSetupClass(Assembly assembly)
        {
            var types = assembly.GetTypesWithAttribute(typeof(SetupAttribute));
            var enumerator = types.GetEnumerator();
            if (enumerator.MoveNext())
                return enumerator.Current;

            return null;
        }
    }
}