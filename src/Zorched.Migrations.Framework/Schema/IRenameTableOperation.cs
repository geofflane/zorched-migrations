namespace Zorched.Migrations.Framework.Schema
{
    public interface IRenameTableOperation : ISchemaOperation
    {
        string NewTableName { get; set; }
    }
}