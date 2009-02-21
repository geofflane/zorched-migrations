using NUnit.Framework;
using Zorched.Migrations.Framework;

namespace Zorched.Migrations.Tests
{

    [TestFixture]
    public class OnlyWhenDriverAttributeTests
    {

        [Test]
        public void only_when_attribute_should_run_when_driver_included()
        {
            var mi = typeof(OnlyWhenTestClass).GetMethod("Foo1");
            Assert.IsTrue(OnlyWhenDriverAttribute.ShouldRun(mi, "Test1"));
            Assert.IsTrue(OnlyWhenDriverAttribute.ShouldRun(mi, "Test3"));
        }

        [Test]
        public void only_when_attribute_should_not_run_when_driver_not_included()
        {
            var mi = typeof(OnlyWhenTestClass).GetMethod("Foo1");
            Assert.IsFalse(OnlyWhenDriverAttribute.ShouldRun(mi, "NotThere"));
        }

        [Test]
        public void only_when_should_run_when_no_attribute()
        {
            var mi = typeof(OnlyWhenTestClass).GetMethod("Foo2");
            Assert.IsTrue(OnlyWhenDriverAttribute.ShouldRun(mi, "NotThere"));
        }

        private class OnlyWhenTestClass
        {
            [Down(1)]
            [OnlyWhenDriver("Test1;Test3")]
            public void Foo1()
            {
                
            }

            [Down(2)]
            public void Foo2()
            {

            }
        }
    }
}