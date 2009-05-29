// Copyright 2008, 2009 Geoff Lane <geoff@zorched.net>
// 
// This file is part of Zorched Migrations a SQL Schema Migration framework for .NET.
// 
// Zorched Migrations is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Zorched Migrations is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Zorched.Migrations.Core.Extensions;
using Zorched.Migrations.Framework;

namespace Zorched.Migrations.Core
{
    /// <summary>
    /// Class classes and methods marked with the Setup attribute.
    /// </summary>
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