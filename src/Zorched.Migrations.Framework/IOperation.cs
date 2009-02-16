using System.Data;

namespace Zorched.Migrations.Framework
{
    public interface IOperation
    {
        void Execute(IDbCommand command);
    }
}