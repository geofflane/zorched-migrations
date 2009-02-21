using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Zorched.Migrations.Core.Extensions;
using Zorched.Migrations.Framework;
using Zorched.Migrations.Framework.Extensions;

namespace Zorched.Migrations.Core
{
    public class Migration : IMigration
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

        public virtual void Setup(IOperationRepository driver)
        {
            var method = GetSetupMethod(type);
            if (null != method)
            {
                method.Invoke(migration, new []{driver});
            }
        }

        public virtual void Up(IDriver driver, ISchemaInfo schemaInfo)
        {
            driver.BeforeUp(Version);

            var methods = GetUpMethods(migration.GetType(), driver);
            methods.ForEach(method => method.Invoke(migration, new[] {driver}));

            driver.AfterUp(Version);

            schemaInfo.InsertSchemaVersion(Version);
        }

        public virtual void Down(IDriver driver, ISchemaInfo schemaInfo)
        {
            driver.BeforeDown(Version);

            var methods = GetDownMethods(migration.GetType(), driver);
            methods.ForEach(method => method.Invoke(migration, new[] {driver}));

            driver.AfterDown(Version);

            schemaInfo.DeleteSchemaVersion(Version);
        }

        public MethodInfo GetSetupMethod(Type t)
        {
            IEnumerable<MethodInfo> methods = t.GetMethodsWithAttribute(typeof(SetupAttribute));
            if (null == methods || 0 == methods.ToList().Count)
                return null;

            return methods.ToList()[0];
        }

        public IEnumerable<MethodInfo> GetUpMethods(Type t, IDriver driver)
        {
            var methods = t.GetMethodsWithAttribute(typeof (UpAttribute));
            return methods
                .Where(mi => OnlyWhenDriverAttribute.ShouldRun(mi, driver.DriverName))
                .OrderBy(mi => UpAttribute.GetOrder(mi));
        }

        public IEnumerable<MethodInfo> GetDownMethods(Type t, IDriver driver)
        {
            var methods = t.GetMethodsWithAttribute(typeof (DownAttribute));
            return methods
                    .Where(mi => OnlyWhenDriverAttribute.ShouldRun(mi, driver.DriverName))
                    .OrderBy(mi => DownAttribute.GetOrder(mi));
        }
    }
}