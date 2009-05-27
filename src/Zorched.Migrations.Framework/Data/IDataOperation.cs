using System.Collections.Generic;

namespace Zorched.Migrations.Framework.Data
{
    /// <summary>
    /// The base operation for dealing with Data operations on a database.
    /// </summary>
    public interface IDataOperation : IOperation
    {
        string SchemaName { get; set; }
        string TableName { get; set; }

        IList<string> Columns { get; }
    }
}