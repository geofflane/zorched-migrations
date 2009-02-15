using System.Data;

namespace Zorched.Migrations.Framework.Schema
{
    public interface ISchemaOperation
    {
        string SchemaName { get; set; }
        string TableName { get; set; }

        void Execute(IDbCommand command);
    }
}