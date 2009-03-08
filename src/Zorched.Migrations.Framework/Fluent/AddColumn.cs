using Zorched.Migrations.Framework.Schema;

namespace Zorched.Migrations.Framework.Fluent
{
    public class AddColumn : IColumnUser
    {
        private IDriver driver;
        private readonly IOperationRepository oprepository;
        private readonly IAddColumnOperation addColumnOp;

        public AddColumn(IDriver driver, IOperationRepository oprepos)
        {
            this.driver = driver;
            oprepository = oprepos;
            addColumnOp = oprepository.InstanceForInteface<IAddColumnOperation>();
        }

        public void SetColumn(Column c)
        {
            addColumnOp.Column = c;
        }

        public AddColumn UsingSchema(string schema)
        {
            addColumnOp.SchemaName = schema;
            return this;
        }

        public AddColumn ToTable(string name)
        {
            addColumnOp.TableName = name;
            return this;
        }

        public TableColumn<AddColumn> Column
        {
            get
            {
                Add();
                return new TableColumn<AddColumn>(new AddColumn(driver, oprepository));
            }
        }

        public void Add()
        {
            Run();
        }

        public void Run()
        {
            driver.Run(addColumnOp);
        }
    }
}