using Zorched.Migrations.Framework;

namespace Zorched.Migrations.Core
{
    public interface IMigration
    {
        long Version { get; }

        void Up(IDriver driver, ISchemaInfo schemaInfo);

        void Down(IDriver driver, ISchemaInfo schemaInfo);
    }
}