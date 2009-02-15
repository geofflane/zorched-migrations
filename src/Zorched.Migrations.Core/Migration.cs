using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Zorched.Migrations.Core.Extensions;
using Zorched.Migrations.Framework;
using Zorched.Migrations.Framework.Extensions;

namespace Zorched.Migrations.Core
{
    public class Migration
    {
        private readonly object migration;
        private readonly Type type;

        public Migration(Type t)
        {
            type = t;
            ConstructorInfo ci = t.GetConstructor(Type.EmptyTypes);
            migration = ci.Invoke(null);
        }

        public long Version
        {
            get { return MigrationAttribute.GetVersion(type); }
        }

        public void Up(IDriver driver)
        {
            var methods = GetUpMethods(migration.GetType());
            methods.ForEach(method => method.Invoke(migration, new[] { driver }));
        }

        public void Down(IDriver driver)
        {
            var methods = GetDownMethods(migration.GetType());
            methods.ForEach(method => method.Invoke(migration, new[] { driver }));
        }

        public IEnumerable<MethodInfo> GetUpMethods(Type t)
        {
            var methods = t.GetMethodsWithAttribute(typeof (UpAttribute));
            return methods.OrderBy(mi => UpAttribute.GetOrder(mi));
        }

        public IEnumerable<MethodInfo> GetDownMethods(Type t)
        {
            var methods = t.GetMethodsWithAttribute(typeof (DownAttribute));
            return methods.OrderBy(mi => DownAttribute.GetOrder(mi));
        }
    }
}