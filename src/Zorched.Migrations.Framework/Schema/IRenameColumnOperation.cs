namespace Zorched.Migrations.Framework.Schema
{
    /// <summary>
    /// The interface for renaming schema columns.
    /// </summary>
    public interface IRenameColumnOperation : ISchemaOperation
    {
        string ColumnName { get; set; }
        string NewColumnName { get; set; }
    }
}