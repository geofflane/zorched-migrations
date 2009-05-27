using System.Collections.Generic;

namespace Zorched.Migrations.Framework.Data
{
    /// <summary>
    /// The interface that needs to be implemented to handle update
    /// operations on the database.
    /// </summary>
    public interface IUpdateOperation : IDataOperation
    {
        IList<object> Values { get; }

        void Where(params Restriction[] restrictions);

        void Where(string rawClause);

        void Where(string column, object val);
    }
}