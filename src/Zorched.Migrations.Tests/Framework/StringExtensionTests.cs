using System.Text;
using NUnit.Framework;
using Zorched.Migrations.Framework.Extensions;

namespace Zorched.Migrations.Tests.Framework
{
    [TestFixture]
    public class StringExtensionTests
    {

        [Test]
        public void can_find_endwith_of_stringbuffer()
        {
            var sb1 = new StringBuilder("foo1234");
            var sb2 = new StringBuilder("foo1233");

            Assert.IsTrue(sb1.EndsWith('4'));
            Assert.IsFalse(sb2.EndsWith('4'));
        }

        [Test]
        public void can_trim_character_end_of_stringbuffer()
        {
            var sb1 = new StringBuilder("foo1234");
            var sb2 = new StringBuilder("foo1233");
            Assert.AreEqual("foo123", sb1.TrimEnd('4').ToString());
            Assert.AreEqual("foo1233", sb2.TrimEnd('4').ToString());
        }

        [Test]
        public void can_trim_end_of_stringbuffer()
        {
            var sb1 = new StringBuilder("foo1234   \r\n");
            var sb2 = new StringBuilder("foo1234");
            Assert.AreEqual("foo1234", sb1.TrimEnd().ToString());
            Assert.AreEqual("foo1234", sb2.TrimEnd().ToString());
        }
    }
}