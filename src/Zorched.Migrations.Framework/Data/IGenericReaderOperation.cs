namespace Zorched.Migrations.Framework.Data
{
    /// <summary>
    /// The interface that can be used to implement any kind of
    /// reader operation that is not otherwise supported.
    /// </summary>
    public interface IGenericReaderOperation : IReaderOperation
    {
        string Sql { get; set; }
    }
}