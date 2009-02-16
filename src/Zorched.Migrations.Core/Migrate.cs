using System.Collections.Generic;
using Zorched.Migrations.Framework;

namespace Zorched.Migrations.Core
{
    public class Migrate
    {

        private readonly DriverLoader driverLoader = new DriverLoader();
        private readonly MigrationLoader migrationLoader = new MigrationLoader();
        private readonly SchemaInfo schemaInfo;

        public Migrate(string driverAssembly, string connectionString)
        {
            var assembly = driverLoader.GetAssembly(driverAssembly);
            Driver = driverLoader.GetDriver(assembly, connectionString);
            Migrations = new List<Migration>(migrationLoader.GetMigrations(assembly));
            schemaInfo = new SchemaInfo(Driver);
            AppliedMigrations = schemaInfo.AppliedMigrations();
        }

        public IDriver Driver { get; set; }
        public IList<Migration> Migrations { get; protected set; }
        public IList<long> AppliedMigrations { get; protected set; }
        

        public long FirstMigration
        {
            get { return Migrations[0].Version; }
        }

        public long LastMigration
        {
            get { return Migrations[Migrations.Count - 1].Version; }
        }

        public bool VersionAlreadyApplied(long version)
        {
            return AppliedMigrations.Contains(version);
        }
    }
}