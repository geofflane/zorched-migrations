namespace Zorched.Migrations.Framework.Schema
{
    /// <summary>
    /// The interface for removing database constraints.
    /// </summary>
    public interface IDropConstraintOperation : ISchemaOperation
    {
        string ConstraintName { get; set; }
    }
}