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

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Zorched.Migrations.Core;
using ILogger=Zorched.Migrations.Framework.ILogger;

namespace Zorched.Migrations.MSBuild
{
    /// <summary>
    /// Runs migrations on a database
    /// </summary>
    /// <example>
    /// <Target Name="Migrate" DependsOnTargets="Build">
    ///     <Migrate DriverAssembly="Zorched.Migrations.SqlServer"
    ///         Connectionstring="Database=MyDB;Data Source=localhost;User Id=;Password=;"
    ///         Migrations="bin/MyProject.dll"/>
    /// </Target>
    /// </example>
    /// <example>
    /// <Target Name="Migrate" DependsOnTargets="Build">
    ///     <CreateProperty Value="-1"  Condition="'$(SchemaVersion)'==''">
    ///        <Output TaskParameter="Value" PropertyName="SchemaVersion"/>
    ///     </CreateProperty>
    ///     <Migrate DriverAssembly="Zorched.Migrations.SqlServer"
    ///         Connectionstring="Database=MyDB;Data Source=localhost;User Id=;Password=;"
    ///         Migrations="bin/MyProject.dll"
    ///         To="$(SchemaVersion)"/>
    /// </Target>
    /// </example>
    public class Migrate : Task
    {
        public Migrate()
        {
            To = -1;
        }

        [Required]
        public string DriverAssembly { set; get; }

        [Required]
        public string ConnectionString { set; get; }

        /// <summary>
        /// The paths to the assemblies that contain your migrations. 
        /// This will generally just be a single item.
        /// </summary>
        public ITaskItem[] Migrations { set; get; }

        public long To { get; set; }

        /// <summary>
        /// If true, Dryrun will show what migrations will be applied to the database
        /// </summary>
        public bool Dryrun { get; set; }

        /// <summary>
        /// If this value is set then the SQL generated will be logged to the given file.
        /// </summary>
        public string ScriptFile { get; set; }
	    
        public override bool Execute()
        {
            if (null == Migrations)
            {
                Log.LogError("Migrations attribute must be set to one or more Migration Assemblies.");
                return false;
            }

            foreach (ITaskItem assembly in Migrations)
            {
                Execute(assembly.GetMetadata("FullPath"));
            }

            return true;
        }

        private void Execute(string assembly)
        {
            using (var logger = new MSBuildLogger(Log, ScriptFile))
            {
                var migrator = new Migrator(logger, DriverAssembly, assembly, ConnectionString);

                if (Dryrun)
                {
                    LogMigrationsToBeApplied(logger, migrator);
                    return;
                }

                if (To == -1)
                    migrator.MigrateTo();
                else
                    migrator.MigrateTo(To);
            }
        }

        private static void LogMigrationsToBeApplied(ILogger logger, Migrator migrator)
        {
            logger.LogInfo("Migrations to be applied:");
            foreach(var migration in migrator.MigrationsToBeApplied)
            {
                logger.LogInfo(string.Format("{0} : {1}", migration.Version, migration.Name));
            }
        }
    }
}