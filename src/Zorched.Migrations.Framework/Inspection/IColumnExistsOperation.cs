
namespace Zorched.Migrations.Framework.Inspection
{
    /// <summary>
    /// The interface that needs to be implemented to determine
    /// if a column exists in a schema.
    /// </summary>
    public interface IColumnExistsOperation : IInspectionOperation
    {
        string SchemaName { get; set; }
        string TableName { get; set; }
        string ColumnName { get; set; }
    }
}