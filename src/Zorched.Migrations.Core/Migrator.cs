using System;
using System.Collections.Generic;
using Zorched.Migrations.Framework;
using Zorched.Migrations.Framework.Extensions;

namespace Zorched.Migrations.Core
{
    public class Migrator
    {
        private readonly SetupRunner setupRunner = new SetupRunner();
        private readonly DriverLoader driverLoader = new DriverLoader();
        private readonly MigrationLoader migrationLoader = new MigrationLoader();
        private readonly ISchemaInfo schemaInfo;
        private readonly string migrationAssemblyName;

        public Migrator(ILogger logger, string driverAssembly, string migrationsAssemblyPath, string connectionString)
        {
            Logger = logger;
            var driverAssem = driverLoader.GetAssemblyByName(driverAssembly);
            var migrationsAssem = driverLoader.GetAssemblyFromPath(migrationsAssemblyPath);
            migrationAssemblyName = migrationsAssem.FullName;

            Driver = driverLoader.GetDriver(driverAssem, connectionString, Logger);
            SetupType = SetupAttribute.GetSetupClass(driverAssem);

            Migrations = new Dictionary<long, IMigration>();
            migrationLoader.GetMigrations(migrationsAssem).ForEach(m => Migrations.Add(m.Version, m));

            schemaInfo = new SchemaInfo(Driver);
            AppliedMigrations = schemaInfo.AppliedMigrations(migrationAssemblyName);
        }

        public IDriver Driver { get; set; }
        public ILogger Logger { get; set; }
        public Type SetupType { get; set; }

        public IDictionary<long, IMigration> Migrations { get; protected set; }
        public List<long> AppliedMigrations { get; protected set; }

        public List<IMigration> MigrationsToBeApplied
        {
            get
            {
                var toBeApplied = new List<IMigration>();
                foreach (var mig in Migrations.Values)
                {
                    if (! VersionAlreadyApplied(mig.Version))
                    {
                        toBeApplied.Add(mig);
                    }
                }
                return toBeApplied;
            }
        }
        
        public void MigrateTo()
        {
            MigrateTo(LastMigration);
        }

        public void MigrateTo(long version)
        {
            try
            {
                schemaInfo.EnsureSchemaTable();
                RunSetupIfNeeded();

                if (version <= 0)
                    version = long.MaxValue;

                var schemaVersion = schemaInfo.CurrentSchemaVersion(migrationAssemblyName);
                if (version < schemaVersion)
                    MigrateDownTo(schemaVersion, version);
                else
                    MigrateUpTo(schemaVersion, version);
            }
            catch (MigrationContractException mcex)
            {
                Logger.LogError(String.Format("Error in a migration: {0}", mcex.OffendingType.FullName), mcex);
            }
            catch (Exception ex)
            {
                Logger.LogError("Error running migrations", ex);
            }
        }

        private void RunSetupIfNeeded()
        {
            if (null != SetupType)
            {
                setupRunner.Invoke(SetupType, (IOperationRepository) Driver);
            }
        }

        private void MigrateDownTo(long schemaVersion, long newVersion)
        {
            var previousVersion = schemaVersion + 1; // Start ahead and then move back in PreviousMigration
            IMigration migration;
            do
            {
                migration = PreviousMigration(previousVersion);
                if (null != migration)
                {
                    migration.Setup(setupRunner, Logger, (IOperationRepository)Driver);

                    migration.Down(Driver, Logger, schemaInfo);

                    AppliedMigrations.Remove(migration.Version);
                    AppliedMigrations.Sort();

                    previousVersion = migration.Version;
                }

            } while (null != migration && previousVersion >= newVersion);
        }

        private void MigrateUpTo(long schemaVersion, long newVersion)
        {
            var nextVersion = schemaVersion;
            IMigration migration;
            do
            {
                migration = NextMigration(nextVersion);
                if (null != migration)
                {
                    migration.Setup(setupRunner, Logger, (IOperationRepository)Driver);

                    migration.Up(Driver, Logger, schemaInfo);

                    AppliedMigrations.Add(migration.Version);
                    AppliedMigrations.Sort();

                    nextVersion = migration.Version;
                }

            } while (null != migration && nextVersion <= newVersion);
        }

        protected IMigration NextMigration(long current)
        {
            // Start searching at the current index
            IMigration next;
            do
            {
                next = Migrations[current++];
                if (AppliedMigrations.Contains(next.Version))
                    next = null;

            } while (null == next && current < LastMigration);

            return next;
        }

        protected IMigration PreviousMigration(long current)
        {
            // Start searching at the current index
            IMigration next;
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