using System.Linq;
using System.Reflection;
using NUnit.Framework;
using Zorched.Migrations.Framework;

namespace Zorched.Migrations.Tests
{

    [TestFixture]
    public class MigrationAttributeTests
    {
        [Test]
        public void migration_can_get_version()
        {
            Assert.AreEqual(1, MigrationAttribute.GetVersion(typeof(TestMigration1)));
        }

        [Test]
        public void migration_can_find_migrations()
        {
            var migrations = MigrationAttribute.GetTypes(Assembly.GetAssembly(GetType())).ToList();
            Assert.IsNotNull(migrations);
            CollectionAssert.AllItemsAreNotNull(migrations);
        }

        [Test]
        public void migration_returns_migrations_in_order()
        {
            var migrations = MigrationAttribute.GetTypes(Assembly.GetAssembly(GetType())).ToList();
            Assert.IsNotNull(migrations);
            Assert.AreEqual(1, MigrationAttribute.GetVersion(migrations[0]));
            Assert.AreEqual(2, MigrationAttribute.GetVersion(migrations[1]));
        }
    }

    [Migration(1)]
    public class TestMigration1
    {

    }

    [Migration(2)]
    public class TestMigration2
    {

    }
}