
namespace Zorched.Migrations.Framework.Inspection
{
    public interface ITableExistsOperation : IInspectionOperation
    {
        string SchemaName { get; set; }
        string TableName { get; set; }
    }
}