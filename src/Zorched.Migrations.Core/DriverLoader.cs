using System;
using System.Data;
using System.Data.Common;
using System.Reflection;
using Zorched.Migrations.Framework;

namespace Zorched.Migrations.Core
{
    public class DriverLoader
    {

        public Assembly GetAssembly(string assemblyName)
        {
            return Assembly.Load(new AssemblyName(assemblyName));
        }

        public IDriver GetDriver(string assemblyName, string connectionString)
        {
            var assembly = GetAssembly(assemblyName);
            return GetDriver(assembly, connectionString);
        }

        public IDriver GetDriver(Assembly assembly, string connectionString)
        {
            var driverType = DriverAttribute.GetDriver(assembly);
            var connection = GetConnection(driverType, connectionString);

            var constructor = driverType.GetConstructor(new[] {typeof (IDbConnection)});
            return (IDriver) constructor.Invoke(new[] {connection});
        }

        public DbConnection GetConnection(Type driverType, string connectionString)
        {
            var provider = DriverAttribute.GetProvider(driverType);

            var factory = DbProviderFactories.GetFactory(provider);
            var connection = factory.CreateConnection();
            connection.ConnectionString = connectionString;
            return connection;
        }
    }
}