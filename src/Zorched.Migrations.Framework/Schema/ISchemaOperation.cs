
namespace Zorched.Migrations.Framework.Schema
{
    public interface ISchemaOperation : IOperation
    {
        string SchemaName { get; set; }
        string TableName { get; set; }
    }
}