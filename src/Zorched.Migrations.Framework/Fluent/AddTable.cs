using Zorched.Migrations.Framework.Schema;

namespace Zorched.Migrations.Framework.Fluent
{
    public class AddTable : IColumnUser
    {
        private IDriver driver;
        private readonly IOperationRepository oprepository;
        private readonly IAddTableOperation addTableOp;

        public AddTable(IDriver driver, IOperationRepository oprepos)
        {
            this.driver = driver;
            oprepository = oprepos;
            addTableOp = oprepository.InstanceForInteface<IAddTableOperation>();
        }

        public void SetColumn(Column c)
        {
            addTableOp.Columns.Add(c);
        }

        public AddTable UsingSchema(string schema)
        {
            addTableOp.SchemaName = schema;
            return this;
        }

        public AddTable AndName(string name)
        {
            return WithName(name);
        }

        public AddTable WithName(string name)
        {
            addTableOp.TableName = name;
            return this;
        }

        public TableColumn<AddTable> WithColumn
        {
            get { return new TableColumn<AddTable>(this); }
        }

        public AddTable Table { get { return this;} }

        public void Add()
        {
            Run();
        }

        public void Run()
        {
            driver.Run(addTableOp);
        }
    }
}