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

namespace Zorched.Migrations.Framework.Data
{
    /// <summary>
    /// The interface that needs to be implemented to handle select
    /// operations on the database.
    /// </summary>
    public interface ISelectOperation : IReaderOperation
    {
        string SchemaName { get; set; }
        string TableName { get; set; }

        IList<string> Columns { get; }

        void Where(params Restriction[] restriction);

        void Where(string rawClause);

        void Where(string column, object val);
    }
}