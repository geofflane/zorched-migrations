using System.Collections.Generic;

namespace Zorched.Migrations.Framework.Data
{
    public interface IUpdateOperation : IDataOperation
    {
        IList<object> Values { get; }

        void Where(params Restriction[] restrictions);

        void Where(string rawClause);

        void Where(string column, object val);
    }
}