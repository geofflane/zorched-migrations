using System.Collections.Generic;

namespace Zorched.Migrations.Framework.Data
{
    /// <summary>
    /// The interface that needs to be implemented to handle insert
    /// operations on the database.
    /// </summary>
    public interface IInsertOperation : IDataOperation
    {
        IList<object> Values { get; }
    }
}