using NUnit.Framework;
using Zorched.Migrations.Framework;

namespace Zorched.Migrations.Tests
{

    [TestFixture]
    public class UpAttributeTests
    {

        [Test]
        public void up_attribute_can_get_order()
        {
            var mi = typeof (UpTestClass).GetMethod("Foo1");
            Assert.AreEqual(1, UpAttribute.GetOrder(mi));
        }

        private class UpTestClass
        {
            [Up(1)]
            public void Foo1()
            {
                
            }

            [Up(2)]
            public void Foo2()
            {

            }
        }
    }
}