using System.Collections.Generic;

namespace Zorched.Migrations.Framework.Data
{
    public interface IInsertOperation : IDataOperation
    {
        IList<object> Values { get; }
    }
}