using System.Collections.Generic;

namespace Zorched.Migrations.Framework.Data
{
    public interface ISelectOperation : IReaderOperation
    {
        string SchemaName { get; set; }
        string TableName { get; set; }

        IList<string> Columns { get; }

        void Where(params Restriction[] restriction);

        void Where(string rawClause);

        void Where(string column, object val);
    }
}