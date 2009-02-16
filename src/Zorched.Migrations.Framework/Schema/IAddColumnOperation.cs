namespace Zorched.Migrations.Framework.Schema
{
    public interface IAddColumnOperation : ISchemaOperation
    {
        Column Column { get; set; }
    }
}