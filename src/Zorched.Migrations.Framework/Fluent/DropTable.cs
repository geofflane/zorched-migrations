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
    public class DropTable
    {
        private IDriver driver;
        private readonly IOperationRepository oprepository;
        private readonly IDropTableOperation dropTableOp;

        public DropTable(IDriver driver, IOperationRepository oprepos)
        {
            this.driver = driver;
            oprepository = oprepos;
            dropTableOp = oprepository.NewInstance<IDropTableOperation>();
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