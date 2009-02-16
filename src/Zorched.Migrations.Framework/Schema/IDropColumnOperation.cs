namespace Zorched.Migrations.Framework.Schema
{
    public interface IDropColumnOperation : ISchemaOperation
    {
        string ColumnName { get; set; }
    }
}