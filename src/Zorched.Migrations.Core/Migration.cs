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
            if (null == ci)
            {
                throw new MigrationContractException("Migration class must have a no argument constructor.", t);
            }
            migration = ci.Invoke(null);
        }

        public long Version
        {
            get { return MigrationAttribute.GetVersion(type); }
        }

        public virtual void Setup(SetupRunner setupRunner, IOperationRepository driver)
        {
            setupRunner.Invoke(migration, driver);
        }

        public virtual void Up(IDriver driver, ISchemaInfo schemaInfo)
        {
            driver.BeforeUp(Version);

            var methods = GetUpMethods(migration.GetType(), driver);
            try
            {
                methods.ForEach(method => method.Invoke(migration, new[] {driver}));
            }
            catch (ArgumentException)
            {
                throw new MigrationContractException("[Up] methods must take a single IDriver parameter as an argument.", type);    
            }

            driver.AfterUp(Version);

            schemaInfo.InsertSchemaVersion(Version);
        }

        public virtual void Down(IDriver driver, ISchemaInfo schemaInfo)
        {
            driver.BeforeDown(Version);

            var methods = GetDownMethods(migration.GetType(), driver);
            try
            {
                methods.ForEach(method => method.Invoke(migration, new[] {driver}));
            }
            catch (ArgumentException)
            {
                throw new MigrationContractException("[Down] methods must take a single IDriver parameter as an argument.", type);    
            }

            driver.AfterDown(Version);

            schemaInfo.DeleteSchemaVersion(Version);
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