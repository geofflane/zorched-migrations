using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Zorched.Migrations.Framework;
using Zorched.Migrations.Framework.Extensions;

namespace Zorched.Migrations.Core
{
    /// <summary>
    /// Handles loading Migration classes from an Assembly.
    /// </summary>
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
