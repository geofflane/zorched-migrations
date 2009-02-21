using System.Reflection;
using NUnit.Framework;
using Zorched.Migrations.Framework;

namespace Zorched.Migrations.Tests
{

    [TestFixture]
    public class DriverAttributeTests
    {

        [Test]
        public void driver_can_find_driver_name()
        {
            Assert.AreEqual("Test", DriverAttribute.GetDriverName(typeof(DriverTestClass)));
        }

        [Test]
        public void driver_can_find_provider()
        {
            Assert.AreEqual("TestProvider", DriverAttribute.GetProvider(typeof(DriverTestClass)));
        }

        [Test]
        public void driver_can_get_driver_instance()
        {
            var t = DriverAttribute.GetDriver(Assembly.GetAssembly(GetType()));
            Assert.IsNotNull(t);
            Assert.AreEqual("Test", DriverAttribute.GetDriverName(t));
        }

    }

    [Driver("Test", "TestProvider")]
    public class DriverTestClass
    {

    }
}