using System.Collections.Generic;

namespace Zorched.Migrations.Framework.Data
{
    public interface IUpdateOperation : IDataOperation
    {
        IList<object> Values { get; }

        string WhereColumn { get; set; }
        object WhereValue { get; set; }
        string WhereClause { get; set; }
    }
}