using NUnit.Framework;
using Zorched.Migrations.Core.Extensions;

namespace Zorched.Migrations.Tests.Core
{
    [TestFixture]
    public class TypeExtensionTests
    {
        [Test]
        public void type_to_human_name()
        {
            Assert.AreEqual("Type extension tests", GetType().ToHumanName());
        }
    }
}