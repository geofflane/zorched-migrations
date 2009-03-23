namespace Zorched.Migrations.Framework.Schema
{
    public interface IAddUniqueConstraintOperation : ISchemaOperation
    {
        string ColumnName { get; set; }
        string ConstraintName { get; set; }
        ConstraintProperty Property { get; set; }
    }
}