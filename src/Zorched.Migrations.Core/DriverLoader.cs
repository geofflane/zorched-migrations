using System;
using System.Data;
using System.Data.Common;
using System.Reflection;
using Zorched.Migrations.Framework;

namespace Zorched.Migrations.Core
{
    public class DriverLoader
    {

        public Assembly GetAssemblyByName(string assemblyName)
        {
            return Assembly.Load(new AssemblyName(assemblyName));
        }

        public Assembly GetAssemblyFromPath(string assemblyPath)
        {
            return Assembly.LoadFile(assemblyPath);
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

            var constructor = driverType.GetConstructor(new[] {typeof (IDbConnection), typeof(ILogger)});
            return (IDriver) constructor.Invoke(new object[] {connection, logger});
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