namespace Zorched.Migrations.Framework.Data
{
    public interface IDeleteOperation : IOperation
    {
        string SchemaName { get; set; }
        string TableName { get; set; }

        string WhereColumn { get; set; }
        object WhereValue { get; set; }
        string WhereClause { get; set; }
    }
}