using System.Linq;
using System.Reflection;
using NUnit.Framework;
using Zorched.Migrations.Core;

namespace Zorched.Migrations.Tests
{
    [TestFixture]
    public class MigrationLoaderTests
    {
        [Test]
        public void migration_loader_returns_migrations_in_order()
        {
            var ml = new MigrationLoader();
            var migrations = ml.GetMigrations(Assembly.GetAssembly(GetType())).ToList();
            Assert.IsNotNull(migrations);
            Assert.AreEqual(1, migrations[0].Version);
            Assert.AreEqual(2, migrations[1].Version);
        }
    }
}