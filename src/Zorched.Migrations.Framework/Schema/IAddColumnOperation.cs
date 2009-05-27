namespace Zorched.Migrations.Framework.Schema
{
    /// <summary>
    /// The interface for adding a column to an existing table.
    /// </summary>
    public interface IAddColumnOperation : ISchemaOperation
    {
        Column Column { get; set; }
    }
}