
namespace Zorched.Migrations.Framework.Inspection
{
    public interface IColumnExistsOperation : IInspectionOperation
    {
        string SchemaName { get; set; }
        string TableName { get; set; }
        string ColumnName { get; set; }
    }
}