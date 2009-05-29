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