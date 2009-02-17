using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Zorched.Migrations.Core;

namespace Zorched.Migrations.MSBuild
{
    /// <summary>
    /// Runs migrations on a database
    /// </summary>
    /// <example>
    /// <Target name="Migrate" DependsOnTargets="Build">
    ///     <Migrate Provider="SqlServer"
    ///         Connectionstring="Database=MyDB;Data Source=localhost;User Id=;Password=;"
    ///         Migrations="bin/MyProject.dll"/>
    /// </Target>
    /// </example>
    /// <example>
    /// <Target name="Migrate" DependsOnTargets="Build">
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

        /// <summary>
        /// The paths to the directory that contains your migrations. 
        /// This will generally just be a single item.
        /// </summary>
        public string Directory { set; get; }

        public string Language { set; get; }

        public long To { get; set; }


        public override bool Execute()
        {
            if (null != Migrations)
            {
                foreach (ITaskItem assembly in Migrations)
                {
                    Execute(assembly.GetMetadata("FullPath"));
                }
            }

            return true;
        }

        private void Execute(string assembly)
        {
            var mig = new Migrator(DriverAssembly, assembly, ConnectionString);

            if (To == -1)
                mig.MigrateTo();
            else
                mig.MigrateTo(To);
        }
    }
}