namespace Zorched.Migrations.Framework.Schema
{
    /// <summary>
    /// The interface for adding a check constraint to the schema.
    /// </summary>
    public interface IAddCheckConstraintOperation : ISchemaOperation
    {
        string ConstraintDefinition { get; set; }
        string ConstraintName { get; set; }
    }
}