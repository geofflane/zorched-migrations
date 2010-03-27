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
using System.Data;

namespace Zorched.Migrations.Framework.Schema
{
    /// <summary>
    /// The interface for adding a table to the schema.
    /// </summary>
    public interface IAddTableOperation : ISchemaOperation
    {
        IList<Column> Columns { get; }
        void AddColumn(Column c);
        void AddColumn(string name, DbType type);
        void AddColumn(string name, DbType type, ColumnProperty property);
        void AddColumn(string name, DbType type, ColumnProperty property, object defaultValue);
        void AddColumn(string name, DbType type, int size, ColumnProperty property);
        void AddColumn(string name, DbType type, int size, object defaultValue);
        void AddColumn(string name, DbType type, int size);

    }
}