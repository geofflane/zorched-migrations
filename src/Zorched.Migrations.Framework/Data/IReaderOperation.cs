using System.Data;

namespace Zorched.Migrations.Framework.Data
{
    public interface IReaderOperation
    {
        IDataReader Execute(IDbCommand command);
    }
}