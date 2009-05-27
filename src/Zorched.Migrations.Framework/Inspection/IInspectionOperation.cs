using System.Data;

namespace Zorched.Migrations.Framework.Inspection
{
    /// <summary>
    /// A base interface that defines an Operation that is used
    /// to query an existing schema.
    /// </summary>
    public interface IInspectionOperation
    {
        bool Execute(IDbCommand command);
    }
}