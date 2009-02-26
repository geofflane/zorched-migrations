using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Zorched.Migrations.Core.Extensions;
using Zorched.Migrations.Framework;

namespace Zorched.Migrations.Core
{
    public class SetupRunner
    {
        public void Invoke(object instance, IOperationRepository driver)
        {
            var method = GetSetupMethod(instance.GetType());
            // It's ok not to have a setup method
            if (null != method)
            {
                method.Invoke(instance, new[] { driver });
            }
        }

        public void Invoke(Type type, IOperationRepository driver)
        {
            ConstructorInfo ci = type.GetConstructor(Type.EmptyTypes);
            if (null == ci)
            {
                throw new MigrationContractException("Setup class must have a no argument constructor.", type);
            }

            var instance = ci.Invoke(null);
            Invoke(instance, driver);
        }

        public MethodInfo GetSetupMethod(Type t)
        {
            IEnumerable<MethodInfo> methods = t.GetMethodsWithAttribute(typeof(SetupAttribute));
            if (null == methods || 0 == methods.ToList().Count)
                return null;

            return methods.ToList()[0];
        }
    }
}