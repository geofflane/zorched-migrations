using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Zorched.Migrations.Core.Extensions
{
    public static class TypeExtensions
    {
        public static IEnumerable<MethodInfo> GetMethodsWithAttribute(this Type t, Type attributeType)
        {
            var methods = t.GetMethods();
            return methods.Where(m => Attribute.IsDefined(m, attributeType, true));
        }
    }
}