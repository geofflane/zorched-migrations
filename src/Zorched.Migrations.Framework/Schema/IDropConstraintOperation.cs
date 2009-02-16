namespace Zorched.Migrations.Framework.Schema
{
    public interface IDropConstraintOperation : ISchemaOperation
    {
        string ConstraintName { get; set; }
    }
}