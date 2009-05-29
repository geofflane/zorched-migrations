// Copyright 2008, 2009 Geoff Lane <geoff@zorched.net>
// 
// This file is part of Zorched Migrations a SQL Schema Migration framework for .NET.
// 
// Zorched Migrations is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Zorched Migrations is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

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