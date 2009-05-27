using System.Data;

namespace Zorched.Migrations.Framework
{
    /// <summary>
    /// The most basic interface of an Operation that can be run against
    /// a database.
    /// </summary>
    public interface IOperation
    {
        void Execute(IDbCommand command);

        string ToString();
    }
}