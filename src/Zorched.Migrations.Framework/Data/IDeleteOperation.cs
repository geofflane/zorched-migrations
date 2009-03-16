namespace Zorched.Migrations.Framework.Data
{
    public interface IDeleteOperation : IOperation
    {
        string SchemaName { get; set; }
        string TableName { get; set; }

        void Where(params Restriction[] restriction);

        void Where(string rawClause);

        void Where(string column, object val);
    }
}