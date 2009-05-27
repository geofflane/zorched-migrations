namespace Zorched.Migrations.Framework.Schema
{
    /// <summary>
    /// The interface for adding a foreign key constaint to the schema.
    /// </summary>
    public interface IAddForeignKeyOperation : ISchemaOperation
    {
        string ConstraintName { get; set; }
        string ColumnName { get; set; }
        ConstraintProperty Property { get; set; }

        string ReferenceSchemaName { get; set; }
        string ReferenceTableName { get; set; }
        string ReferenceColumnName { get; set; }
    }
}