using System;
using System.Configuration;
using System.Reflection;
using NUnit.Framework;
using Zorched.Migrations.Core;
using Zorched.Migrations.Framework;

namespace Zorched.Migrations.Tests.Framework
{
    [TestFixture]
    public class DriverLoaderTests
    {
        private readonly DriverLoader dl = new DriverLoader();

        [Test]
        public void can_get_assembly_by_name()
        {
            GetAssembly();
        }

        [Test]
        public void can_get_driver_from_assembly()
        {
            var cs = ConfigurationManager.ConnectionStrings["SqlServer"];
            var assembly = GetAssembly();
            var driver = dl.GetDriver(assembly, cs.ConnectionString, new TestLogger());
            Assert.IsNotNull(driver);
        }

        public Assembly GetAssembly()
        {
            var assembly = dl.GetAssemblyByName("Zorched.Migrations.Tests");
            Assert.IsNotNull(assembly);

            return assembly;
        }
    }

    public class TestLogger : ILogger
    {
        public void LogError(string error)
        {
            Console.Error.WriteLine(error);
        }

        public void LogError(string error, Exception ex)
        {
            Console.Error.WriteLine(error);
            Console.Error.WriteLine(ex.Message);
            Console.Error.WriteLine(ex.StackTrace);
        }

        public void LogInfo(string info)
        {
            Console.Out.WriteLine(info);
        }

        public void LogSql(string sql)
        {
            Console.Out.WriteLine(sql);
        }
    }
}