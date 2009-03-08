using Zorched.Migrations.Framework.Schema;

namespace Zorched.Migrations.Framework.Fluent
{
    public class DropTable
    {
        private IDriver driver;
        private readonly IOperationRepository oprepository;
        private readonly IDropTableOperation dropTableOp;

        public DropTable(IDriver driver, IOperationRepository oprepos)
        {
            this.driver = driver;
            oprepository = oprepos;
            dropTableOp = oprepository.InstanceForInteface<IDropTableOperation>();
        }

        public DropTable UsingSchema(string schema)
        {
            dropTableOp.SchemaName = schema;
            return this;
        }

        public DropTable AndName(string name)
        {
            return WithName(name);
        }

        public DropTable WithName(string name)
        {
            dropTableOp.TableName = name;
            return this;
        }

        public void Drop()
        {
            driver.Run(dropTableOp);
        }
    }
}