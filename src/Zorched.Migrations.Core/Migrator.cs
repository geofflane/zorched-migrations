using System;
using System.Collections.Generic;
using Zorched.Migrations.Framework;
using Zorched.Migrations.Framework.Extensions;

namespace Zorched.Migrations.Core
{
    /// <summary>
    /// Core implementation that coordinates all of the interactions of the other
    /// classes to successfully run Migrations.
    /// </summary>
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
            SetupType = SetupAttribute.GetSetupClass(migrationsAssem);

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

                if (version < 0)
                    version = long.MaxValue;

                var schemaVersion = schemaInfo.CurrentSchemaVersion(migrationAssemblyName);
                if (version < schemaVersion)
                {
                    Logger.LogInfo("Migrating down...");
                    MigrateDownTo(schemaVersion, version);
                }
                else
                {
                    Logger.LogInfo("Migrating up...");
                    MigrateUpTo(schemaVersion, version);
                }
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
                Logger.LogInfo("Setup class running...");
                setupRunner.Invoke(SetupType, (IOperationRepository) Driver);
            }
        }

        private void MigrateDownTo(long schemaVersion, long newVersion)
        {
            IMigration migration;
            do
            {
                migration = PreviousMigration(schemaVersion);
                if (null != migration)
                {
                    migration.Setup(setupRunner, Logger, (IOperationRepository)Driver);

                    migration.Down(Driver, Logger, schemaInfo);

                    AppliedMigrations.Remove(migration.Version);
                    AppliedMigrations.Sort();

                    schemaVersion = migration.Version - 1;
                }

            } while (null != migration && schemaVersion > newVersion);
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
            do
            {
                current++;
                if (Migrations.ContainsKey(current))
                {
                    IMigration next = Migrations[current];
                    if (!VersionAlreadyApplied(next.Version))
                        return next;
                }

            } while (current <= LastMigration);

            return null;
        }

        protected IMigration PreviousMigration(long current)
        {
            // Start searching at the current index
            do
            {
                if (Migrations.ContainsKey(current))
                {
                    return Migrations[current];
                }
                current--;

            } while (current > 0);

            return null;
        }

        public long FirstMigration
        {
            get
            {
                if (null != Migrations && 0 != Migrations.Count)
                    return Migrations[0].Version;
                return 0;
            }
        }

        public long LastMigration
        {
            get
            {
                if (null != Migrations && 0 != Migrations.Count)
                    return Migrations[Migrations.Count - 1].Version;
                return 0;
            }
        }

        public bool VersionAlreadyApplied(long version)
        {
            return AppliedMigrations.Contains(version);
        }
    }
}