namespace Zorched.Migrations.Framework.Schema
{
    /// <summary>
    /// The interface for renaming schema tables.
    /// </summary>
    public interface IRenameTableOperation : ISchemaOperation
    {
        string NewTableName { get; set; }
    }
}