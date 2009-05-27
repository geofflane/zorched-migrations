using Zorched.Migrations.Framework;

namespace Zorched.Migrations.Core
{
    /// <summary>
    /// Interface that gets implemented to handle running migrations.
    /// </summary>
    public interface IMigration
    {
        long Version { get; }
        string Name { get; }

        void Setup(SetupRunner setupRunner, ILogger logger, IOperationRepository driver);

        void Up(IDriver driver, ILogger logger, ISchemaInfo schemaInfo);

        void Down(IDriver driver, ILogger logger, ISchemaInfo schemaInfo);
    }
}