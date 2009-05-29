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

        void EnsureSchemaTable();

        void CreateSchemaTable();

        long CurrentSchemaVersion(string assembly);

        List<long> AppliedMigrations(string assembly);

        void InsertSchemaVersion(long version, string assembly, string name);

        void DeleteSchemaVersion(long version, string assembly);
    }
}