namespace Zorched.Migrations.Framework.Schema
{
    public interface IChangeColumnOperation : ISchemaOperation
    {
        Column Column { get; set; }        
    }
}