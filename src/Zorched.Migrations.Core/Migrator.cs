using System.Collections.Generic;
using Zorched.Migrations.Framework;
using Zorched.Migrations.Framework.Extensions;

namespace Zorched.Migrations.Core
{
    public class Migrator
    {
        private readonly DriverLoader driverLoader = new DriverLoader();
        private readonly MigrationLoader migrationLoader = new MigrationLoader();
        private readonly SchemaInfo schemaInfo;

        public Migrator(string driverAssembly, string migrationsAssemblyPath, string connectionString)
        {
            var driverAssem = driverLoader.GetAssemblyByName(driverAssembly);
            var migrationsAssem = driverLoader.GetAssemblyFromPath(migrationsAssemblyPath);

            Driver = driverLoader.GetDriver(driverAssem, connectionString);

            Migrations = new Dictionary<long, Migration>();
            migrationLoader.GetMigrations(migrationsAssem).ForEach(m => Migrations.Add(m.Version, m));

            schemaInfo = new SchemaInfo(Driver);
            AppliedMigrations = schemaInfo.AppliedMigrations();
        }

        public IDriver Driver { get; set; }

        public IDictionary<long, Migration> Migrations { get; protected set; }
        public List<long> AppliedMigrations { get; protected set; }
        
        public void MigrateTo()
        {
            MigrateTo(LastMigration);
        }

        public void MigrateTo(long version)
        {
            if (version <= 0)
                version = long.MaxValue;

            var schemaVersion = schemaInfo.CurrentSchemaVersion();
            if (version < schemaVersion)
                MigrateDownTo(schemaVersion, version);
            else
                MigrateUpTo(schemaVersion, version);

        }

        private void MigrateDownTo(long schemaVersion, long newVersion)
        {
            var previousVersion = schemaVersion + 1; // Start ahead and then move back in PreviousMigration
            Migration migration = null;
            do
            {
                migration = PreviousMigration(previousVersion);
                if (null != migration)
                {
                    migration.Down(Driver, schemaInfo);

                    AppliedMigrations.Remove(migration.Version);
                    AppliedMigrations.Sort();

                    previousVersion = migration.Version;
                }

            } while (null != migration && previousVersion >= newVersion);
        }

        private void MigrateUpTo(long schemaVersion, long newVersion)
        {
            var nextVersion = schemaVersion;
            Migration migration = null;
            do
            {
                migration = NextMigration(nextVersion);
                if (null != migration)
                {
                    migration.Up(Driver, schemaInfo);

                    AppliedMigrations.Add(migration.Version);
                    AppliedMigrations.Sort();

                    nextVersion = migration.Version;
                }

            } while (null != migration && nextVersion <= newVersion);
        }

        protected Migration NextMigration(long current)
        {
            // Start searching at the current index
            Migration next = null;
            do
            {
                next = Migrations[current++];
                if (AppliedMigrations.Contains(next.Version))
                    next = null;

            } while (null == next && current < LastMigration);

            return next;
        }

        protected Migration PreviousMigration(long current)
        {
            // Start searching at the current index
            Migration next = null;
            do
            {
                next = Migrations[current--];
                if (AppliedMigrations.Contains(next.Version))
                    next = null;

            } while (null == next && current > 0);

            return next;
        }

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