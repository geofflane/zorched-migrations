
namespace Zorched.Migrations.Framework.Schema
{
    /// <summary>
    /// The base interface for Operations that modify
    /// the database schema.
    /// </summary>
    public interface ISchemaOperation : IOperation
    {
        string SchemaName { get; set; }
        string TableName { get; set; }
    }
}