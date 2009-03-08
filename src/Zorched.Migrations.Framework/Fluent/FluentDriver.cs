namespace Zorched.Migrations.Framework.Fluent
{
    public class FluentDriver
    {
        private readonly IDriver driver;
        public FluentDriver(IDriver driver)
        {
            this.driver = driver;
        }

        public AddTable AddTable
        {
            get { return new AddTable(driver, (IOperationRepository) driver); }
        }

        public AddColumn AddColumn
        {
            get { return new AddColumn(driver, (IOperationRepository)driver); }
        }

        public DropTable DropTable
        {
            get { return new DropTable(driver, (IOperationRepository)driver); }
        }
    }
}