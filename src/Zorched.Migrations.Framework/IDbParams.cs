using System.Data;

namespace Zorched.Migrations.Framework
{
    public interface IDbParams
    {
        IDbConnection Connection { get; }
        IDbTransaction Transaction { get; }

        IDbCommand CreateCommand();
        IDbTransaction BeginTransaction();
    }
}