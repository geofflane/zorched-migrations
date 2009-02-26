using System.Reflection;
using NUnit.Framework;
using Zorched.Migrations.Core;

namespace Zorched.Migrations.Tests
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
            var assembly = GetAssembly();
            var driver = dl.GetDriver(assembly, "Data Source=localhost;Initial Catalog=Northwind;User Id=sq;Password=sql;");
            Assert.IsNotNull(driver);
        }

        public Assembly GetAssembly()
        {
            var assembly = dl.GetAssemblyByName("Zorched.Migrations.Tests");
            Assert.IsNotNull(assembly);

            return assembly;
        }
    }
}