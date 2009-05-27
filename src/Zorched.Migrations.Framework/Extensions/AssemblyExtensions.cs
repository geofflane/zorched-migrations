using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Zorched.Migrations.Framework.Extensions
{
    /// <summary>
    /// Extension methods for dealing with Assembly types.
    /// </summary>
    public static class AssemblyExtensions
    {
        public static IEnumerable<Type> GetTypesWithAttribute(this Assembly assembly, Type attributeType)
        {
            var types = assembly.GetExportedTypes();
            return types.Where(t => Attribute.IsDefined(t, attributeType, true));
        }
    }
}
