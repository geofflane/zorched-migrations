namespace Zorched.Migrations.Framework.Schema
{
    /// <summary>
    /// The interface for changing a column in some way.
    /// </summary>
    public interface IChangeColumnOperation : ISchemaOperation
    {
        Column Column { get; set; }        
    }
}