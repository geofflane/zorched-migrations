// Copyright 2008, 2009 Geoff Lane <geoff@zorched.net>
// 
// This file is part of Zorched Migrations a SQL Schema Migration framework for .NET.
// 
// Zorched Migrations is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Zorched Migrations is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Linq;
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

        /// <summary>
        /// Construct a new Migrator.
        /// This is the main entry point and the core algorighm used to run migrations against a database.
        /// </summary>
        /// <param name="logger">An ILogger to use in the system to print info, errors and SQL output.</param>
        /// <param name="driverAssembly">The assembly name of the Driver assembly that contains the implementation for your database.</param>
        /// <param name="migrationsAssemblyPath">The path to a DLL containing Migrations.</param>
        /// <param name="connectionString">The database connection string needed to connect to the database.</param>
        public Migrator(ILogger logger, string driverAssembly, string migrationsAssemblyPath, string connectionString)
        {
            Logger = logger;
            var driverAssem = driverLoader.GetAssemblyByName(driverAssembly);
            var migrationsAssem = driverLoader.GetAssemblyFromPath(migrationsAssemblyPath);
            migrationAssemblyName = migrationsAssem.GetName().Name;

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

        /// <summary>
        /// The Migration versions that are available in the Migration Assembly but are not yet applied to the database.
        /// </summary>
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
        
        /// <summary>
        /// Perform migrations to the latest version possible
        /// </summary>
        public void MigrateTo()
        {
            MigrateTo(LastMigration);
        }

        /// <summary>
        /// Perform migrations, either up or down, until the version is equal to the version specified.
        /// </summary>
        /// <param name="version">The version that the database should be after this is run.</param>
        public void MigrateTo(long version)
        {
            try
            {
                schemaInfo.EnsureSchemaTable();
                RunSetupIfNeeded();

                if (version < 0)
                    version = long.MaxValue;

                var schemaVersion = schemaInfo.CurrentSchemaVersion(migrationAssemblyName);
                Logger.LogInfo("Current DB Version: " + schemaVersion);
                if (version == schemaVersion)
                {
                    Logger.LogInfo("Current DB Version and Migration version are the same. Nothing to do.");
                }
                else if (version < schemaVersion)
                {
                    Logger.LogInfo("Migrating down to version: " + version);
                    MigrateDownTo(schemaVersion, version);
                }
                else
                {
                    Logger.LogInfo("Migrating up to version: " + version);
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
                    IMigration next = Migrations[current];
                    if (VersionAlreadyApplied(next.Version))
                        return next;
                }
                current--;

            } while (current > 0);

            return null;
        }

        /// <summary>
        /// The lowest version number of the migrations in the loaded assembly.
        /// </summary>
        public long FirstMigration
        {
            get
            {
                if (null != Migrations && 0 != Migrations.Count)
                    return Migrations.Values.First().Version;
                return 0;
            }
        }

        /// <summary>
        /// The highest version number of the migrations in the loaded assembly.
        /// </summary>
        public long LastMigration
        {
            get
            {
                if (null != Migrations && 0 != Migrations.Count)
                    return Migrations.Values.Last().Version;
                return 0;
            }
        }

        /// <summary>
        /// Has the gien version already been applied to the database?
        /// </summary>
        /// <param name="version">The migration version to check</param>
        /// <returns>True if the given migration was already applied.</returns>
        public bool VersionAlreadyApplied(long version)
        {
            return AppliedMigrations.Contains(version);
        }
    }
}