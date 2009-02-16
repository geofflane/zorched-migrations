using System.Collections.Generic;

namespace Zorched.Migrations.Framework.Data
{
    public interface IDataOperation : IOperation
    {
        string SchemaName { get; set; }
        string TableName { get; set; }

        IList<string> Columns { get; }
    }
}