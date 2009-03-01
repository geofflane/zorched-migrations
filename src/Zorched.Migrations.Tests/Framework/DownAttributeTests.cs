using NUnit.Framework;
using Zorched.Migrations.Framework;

namespace Zorched.Migrations.Tests.Framework
{
    [TestFixture]
    public class DownAttributeTests
    {

        [Test]
        public void down_attribute_can_get_order()
        {
            var mi = typeof (DownTestClass).GetMethod("Foo1");
            Assert.AreEqual(1, DownAttribute.GetOrder(mi));
        }

        private class DownTestClass
        {
            [Down(1)]
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