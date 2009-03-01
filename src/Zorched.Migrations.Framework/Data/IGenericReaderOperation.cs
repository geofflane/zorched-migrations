namespace Zorched.Migrations.Framework.Data
{
    public interface IGenericReaderOperation : IReaderOperation
    {
        string Sql { get; set; }
    }
}