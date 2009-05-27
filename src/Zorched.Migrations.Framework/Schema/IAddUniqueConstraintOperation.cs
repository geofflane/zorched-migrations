namespace Zorched.Migrations.Framework.Schema
{
    /// <summary>
    /// The interface for adding a unique constraint to the database.
    /// </summary>
    public interface IAddUniqueConstraintOperation : ISchemaOperation
    {
        string ColumnName { get; set; }
        string ConstraintName { get; set; }
        ConstraintProperty Property { get; set; }
    }
}