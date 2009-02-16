using System.Data;

namespace Zorched.Migrations.Framework.Data
{
    public interface IGenericReaderOperation
    {
        IDataReader Execute(IDbCommand command);
    }
}