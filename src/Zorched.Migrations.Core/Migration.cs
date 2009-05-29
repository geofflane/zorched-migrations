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
using Zorched.Migrations.Framework.Extensions;

namespace Zorched.Migrations.Core
{
    /// <summary>
    /// Default, non-transactional implementation of the IMigration interface 
    /// used by the Migrator to handle running Migrations.
    /// </summary>
    public class Migration : IMigration
    {
        private readonly object migration;
        private readonly Type type;

        /// <summary>
        /// Construct a new Migration from the given Type.
        /// </summary>
        /// <remarks>
        /// This class is a wrapper that handles invoking methods on the given type.
        /// </remarks>
        /// <param name="t">The Migration type that should be constrcuted and invoked.</param>
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

        public string Name
        {
            get { return type.ToHumanName(); }
        }

        public virtual void Setup(SetupRunner setupRunner, ILogger logger, IOperationRepository driver)
        {
            setupRunner.Invoke(migration, driver);
        }

        public virtual void Up(IDriver driver, ILogger logger, ISchemaInfo schemaInfo)
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

            schemaInfo.InsertSchemaVersion(Version, type.Assembly.FullName, type.FullName);
        }

        public virtual void Down(IDriver driver, ILogger logger, ISchemaInfo schemaInfo)
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

            schemaInfo.DeleteSchemaVersion(Version, type.Assembly.FullName);
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