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

using Zorched.Migrations.Framework;

namespace Zorched.Migrations.Core
{
    /// <summary>
    /// Interface that gets implemented to handle running migrations.
    /// </summary>
    public interface IMigration
    {
        long Version { get; }
        string Name { get; }

        /// <summary>
        /// Call any methods marked with the SetupAttribute.
        /// </summary>
        /// <param name="setupRunner">A helper class used to invoke methods marked with the SetupAttribute</param>
        /// <param name="logger">The logger currently in use.</param>
        /// <param name="repos">The currently running OperationRepository</param>
        void Setup(SetupRunner setupRunner, ILogger logger, IOperationRepository repos);

        /// <summary>
        /// Call any methods marked with the UpAttribute
        /// </summary>
        /// <param name="driver">The currently executing database Driver.</param>
        /// <param name="logger">The logger currently in use.</param>
        /// <param name="schemaInfo">The SchemaInfo implementation to interact with the SchemaInfo table.</param>
        void Up(IDriver driver, ILogger logger, ISchemaInfo schemaInfo);

        /// <summary>
        /// Call any methods marked with the DownAttribute
        /// </summary>
        /// <param name="driver">The currently executing database Driver.</param>
        /// <param name="logger">The logger currently in use.</param>
        /// <param name="schemaInfo">The SchemaInfo implementation to interact with the SchemaInfo table.</param>
        void Down(IDriver driver, ILogger logger, ISchemaInfo schemaInfo);
    }
}