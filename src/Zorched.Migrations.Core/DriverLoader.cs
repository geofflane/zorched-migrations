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
using System.Data.Common;
using System.Reflection;
using Zorched.Migrations.Framework;

namespace Zorched.Migrations.Core
{
    /// <summary>
    /// Handles all of the things needed to find, load and initialize IDriver implementations.
    /// </summary>
    public class DriverLoader
    {

        public Assembly GetAssemblyByName(string assemblyName)
        {
            return Assembly.Load(new AssemblyName(assemblyName));
        }

        public Assembly GetAssemblyFromPath(string assemblyPath)
        {
            return Assembly.LoadFrom(assemblyPath);
        }

        public IDriver GetDriver(string assemblyPath, string connectionString, ILogger logger)
        {
            var assembly = GetAssemblyFromPath(assemblyPath);
            return GetDriver(assembly, connectionString, logger);
        }

        public IDriver GetDriver(Assembly assembly, string connectionString, ILogger logger)
        {
            var driverType = DriverAttribute.GetDriver(assembly);
            var connection = GetConnection(driverType, connectionString);

            var constructor = driverType.GetConstructor(new[] {typeof (IDbParams), typeof(ILogger)});
            return (IDriver) constructor.Invoke(new object[] {new DbParams(connection), logger});
        }

        private static DbConnection GetConnection(Type driverType, string connectionString)
        {
            var provider = DriverAttribute.GetProvider(driverType);
            var factory = DbProviderFactories.GetFactory(provider);
            var connection = factory.CreateConnection();
            connection.ConnectionString = connectionString;
            connection.Open();
            return connection;
        }
    }
}