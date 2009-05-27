using System;
using System.Reflection;
using Zorched.Migrations.Framework.Extensions;

namespace Zorched.Migrations.Framework
{
    /// <summary>
    /// An attrbute that can mark a class or a method in a Migration as something that
    /// should be run once prior to any Up or Down methods.
    /// </summary>
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