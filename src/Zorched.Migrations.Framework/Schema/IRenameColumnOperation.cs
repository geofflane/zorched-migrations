namespace Zorched.Migrations.Framework.Schema
{
    public interface IRenameColumnOperation : ISchemaOperation
    {
        string ColumnName { get; set; }
        string NewColumnName { get; set; }
    }
}