using System.Data;

namespace Zorched.Migrations.Framework
{
    /// <summary>
    /// Extra database parameters that can be used by Migrations in specific
    /// special cases. e.g. Wanting to stop a transaction.
    /// </summary>
    public interface IDbParams
    {
        int CommandTimeout { get; set; }
        IDbConnection Connection { get; }
        IDbTransaction Transaction { get; }

        IDbCommand CreateCommand();
        IDbTransaction BeginTransaction();
    }
}