using Zorched.Migrations.Framework;

namespace Zorched.Migrations.Core
{
    public interface IMigration
    {
        long Version { get; }

        void Setup(SetupRunner setupRunner, ILogger logger, IOperationRepository driver);

        void Up(IDriver driver, ILogger logger, ISchemaInfo schemaInfo);

        void Down(IDriver driver, ILogger logger, ISchemaInfo schemaInfo);
    }
}