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

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Zorched.Migrations.Framework;
using Zorched.Migrations.Framework.Extensions;

namespace Zorched.Migrations.Core
{
    /// <summary>
    /// Handles loading Migration classes from an Assembly.
    /// </summary>
    public class MigrationLoader
    {
        /// <summary>
        /// Load all of the types that are marked with a MigrationAttribute from the given assembly.
        /// </summary>
        /// <remarks>This method wraps the types into IMigrations so they can be executed with a simple interface.</remarks>
        /// <param name="assembly">The Assembly containing the Migrations.</param>
        /// <returns>A sorted list of IMigrations.</returns>
        public IEnumerable<IMigration> GetMigrations(Assembly assembly)
        {
            var migrationTypes = MigrationAttribute.GetTypes(assembly);
            return migrationTypes
                    .CastAs(t => new TransactionalMigration(t))
                    .CastAs(t => (IMigration)t)
                    .OrderBy(m => m.Version);
        }
    }
}
