using System.Collections.Generic;

namespace Zorched.Migrations.Framework.Data
{
    public interface ISelectOperation : IGenericReaderOperation
    {
        string SchemaName { get; set; }
        string TableName { get; set; }

        IList<string> Columns { get; }

        string WhereColumn { get; set; }
        object WhereValue { get; set; }
        string WhereClause { get; set; }
    }
}