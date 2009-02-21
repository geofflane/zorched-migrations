using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Zorched.Migrations.Framework;
using Zorched.Migrations.Framework.Extensions;

namespace Zorched.Migrations.Core
{
    public class MigrationLoader
    {
        public IEnumerable<IMigration> GetMigrations(Assembly assembly)
        {
            var migrationTypes = MigrationAttribute.GetTypes(assembly);
            return migrationTypes
                    .CastAs(t => new TransactionalMigration(t))
                    .CastAs(t => (IMigration)t)
                    .OrderBy(m => m.Version);
        }
    }
}
