
using Zorched.Migrations.Framework;

namespace Zorched.Migrations.Examples
{
    [Setup]
    public class MigrationSetup
    {
        [Setup]
        public void Setup(IOperationRepository driver)
        {
            driver.Register<AddNewTable.ICustomOp>(typeof(AddNewTable.CustomOp));
        }
    }
}