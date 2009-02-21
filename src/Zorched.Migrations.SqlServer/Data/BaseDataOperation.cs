
using System.Collections.Generic;

namespace Zorched.Migrations.SqlServer.Data
{
    public abstract class BaseDataOperation : SqlBaseOperation
    {
        public const string PARAM_FORMAT = "@{0}";

        protected BaseDataOperation()
        {
            Columns = new List<string>();
            Values = new List<object>();
        }

        public string SchemaName { get; set; }
        public string TableName { get; set; }

        public IList<string> Columns { get; protected set; }
        public IList<object> Values { get; protected set; }

    }
}