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
using Zorched.Migrations.Framework;

namespace Zorched.Migrations.Core
{
    /// <summary>
    /// Interface that defines all of the operations that can be used to
    /// update the SchemaInfo table.
    /// </summary>
    public interface ISchemaInfo
    {
        IDriver Driver { get; set; }

        /// <summary>
        /// Creates the SchemaInfo table that contains information about applied Migrations.
        /// </summary>
        void EnsureSchemaTable();

        /// <summary>
        /// The highest number applied Migration that has been run against the database.
        /// </summary>
        /// <remarks>
        /// Passing the assembly name allows multiple Migration assemblies to coexist.
        /// </remarks>
        /// <param name="assembly">The name of the assembly that the migrations are from.</param>
        /// <returns>The maximum migration version or zero if none are found.</returns>
        long CurrentSchemaVersion(string assembly);


        /// <summary>
        /// Get a list of all of the Migration versions that have been applied to the database.
        /// </summary>
        /// <param name="assembly">The name of the assembly that the migrations are from.</param>
        /// <returns>A List of all of the applied migrations or an empty List if none have been applied.</returns>
        List<long> AppliedMigrations(string assembly);

        /// <summary>
        /// Add a Migration version to the SchemaInfo table. Used when migrating up to a higher version number.
        /// </summary>
        /// <param name="version">The version to add to the SchemaInfo table.</param>
        /// <param name="assembly">The name of the assembly that the migrations are from.</param>
        /// <param name="name">The name of the migration to make it easier for humans to look at the table.</param>
        void InsertSchemaVersion(long version, string assembly, string name);

        /// <summary>
        /// Remove a Migration version from the SchemaInfo table. Used when migrating down to a lower version number.
        /// </summary>
        /// <param name="version">The version to remove.</param>
        /// <param name="assembly">The name of the assembly that the migrations are from.</param>
        void DeleteSchemaVersion(long version, string assembly);
    }
}