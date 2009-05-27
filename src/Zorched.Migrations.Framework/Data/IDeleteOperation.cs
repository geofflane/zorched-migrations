namespace Zorched.Migrations.Framework.Data
{
    /// <summary>
    /// The interface that needs to be implemented to handle delete
    /// operations on the database.
    /// </summary>
    public interface IDeleteOperation : IOperation
    {
        string SchemaName { get; set; }
        string TableName { get; set; }

        void Where(params Restriction[] restriction);

        void Where(string rawClause);

        void Where(string column, object val);
    }
}