using System.Data;

namespace Zorched.Migrations.Framework.Data
{
    /// <summary>
    /// The base interface that needs to be implemented to handle reading
    /// data from the database.
    /// </summary>
    public interface IReaderOperation
    {
        IDataReader Execute(IDbCommand command);
    }
}