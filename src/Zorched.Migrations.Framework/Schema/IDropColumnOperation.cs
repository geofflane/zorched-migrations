namespace Zorched.Migrations.Framework.Schema
{
    /// <summary>
    /// The interface for removing a column from the schema.
    /// </summary>
    public interface IDropColumnOperation : ISchemaOperation
    {
        string ColumnName { get; set; }
    }
}