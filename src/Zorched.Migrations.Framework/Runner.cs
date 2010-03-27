
namespace Zorched.Migrations.Framework
{
    public abstract class Runner
    {
        protected Runner(IDriver driver)
        {
            Driver = driver;
        }

        public IDriver Driver { get; private set; }

    }
}