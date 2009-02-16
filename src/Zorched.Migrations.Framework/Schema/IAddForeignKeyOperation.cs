namespace Zorched.Migrations.Framework.Schema
{
    public interface IAddForeignKeyOperation : ISchemaOperation
    {
        string ConstraintName { get; set; }
        string ColumnName { get; set; }

        string ReferenceSchemaName { get; set; }
        string ReferenceTableName { get; set; }
        string ReferenceColumnName { get; set; }
    }
}