namespace Zorched.Migrations.Framework.Schema
{
    public interface IGenericOperation : IOperation
    {
        string Sql { get; set; }
    }
}