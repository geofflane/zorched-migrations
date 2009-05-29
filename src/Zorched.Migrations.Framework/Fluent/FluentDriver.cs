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