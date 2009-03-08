using System.Data;

namespace Zorched.Migrations.Framework
{
    public interface IDbParams
    {
        int CommandTimeout { get; set; }
        IDbConnection Connection { get; }
        IDbTransaction Transaction { get; }

        IDbCommand CreateCommand();
        IDbTransaction BeginTransaction();
    }
}