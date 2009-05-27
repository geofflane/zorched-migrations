
namespace Zorched.Migrations.Framework.Inspection
{
    /// <summary>
    /// The interface that needs to be implemented to determine
    /// if a table exists in a schema.
    /// </summary>
    public interface ITableExistsOperation : IInspectionOperation
    {
        string SchemaName { get; set; }
        string TableName { get; set; }
    }
}