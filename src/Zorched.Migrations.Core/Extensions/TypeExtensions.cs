using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Zorched.Migrations.Core.Extensions
{
    public static class TypeExtensions
    {
        public static IEnumerable<MethodInfo> GetMethodsWithAttribute(this Type t, Type attributeType)
        {
            var methods = t.GetMethods();
            return methods.Where(m => Attribute.IsDefined(m, attributeType, true));
        }

        public static string ToHumanName(this Type t)
        {
            var className = t.Name;
            string name = Regex.Replace(className, "([A-Z])", " $1").Substring(1);
            return name.Substring(0, 1).ToUpper() + name.Substring(1).ToLower();
        }
    }
}