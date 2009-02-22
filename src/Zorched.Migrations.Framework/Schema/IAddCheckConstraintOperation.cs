namespace Zorched.Migrations.Framework.Schema
{
    public interface IAddCheckConstraintOperation : ISchemaOperation
    {
        string ConstraintDefinition { get; set; }
        string ConstraintName { get; set; }
    }
}