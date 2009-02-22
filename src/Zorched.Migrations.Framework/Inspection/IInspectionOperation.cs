using System.Data;

namespace Zorched.Migrations.Framework.Inspection
{
    public interface IInspectionOperation
    {
        bool Execute(IDbCommand command);
    }
}