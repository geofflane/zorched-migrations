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

using System.Data;

namespace Zorched.Migrations.Framework.Fluent
{
    public class TableColumn<T> where T : IColumnUser
    {
        private T table;
        private Column column;

        public TableColumn(T table)
        {
            column = new Column();
            this.table = table;
            this.table.SetColumn(column);
        }

        public TableColumn<T> That { get { return this; } }

        public TableColumn<T> Is { get { return this; } }

        public TableColumn<T> Named(string name)
        {
            column.Name = name;
            return this;
        }

        public TableColumn<T> WithSize(int size)
        {
            column.Size = size;
            return this;
        }

        public TableColumn<T> OfType(DbType t)
        {
            column.DbType = t;
            return this;
        }

        public TableColumn<T> IsPrimaryKey
        {
            get
            {
                column.Property |= ColumnProperty.PrimaryKey;
                return this;
            }
        }

        public TableColumn<T> Identity
        {
            get
            {
                column.Property |= ColumnProperty.Identity;
                return this;
            }
        }

        public TableColumn<T> IsNull
        {
            get
            {
                column.Property ^= ColumnProperty.NotNull;
                column.Property |= ColumnProperty.Null;
                return this;
            }
        }

        public TableColumn<T> IsNotNull
        {
            get
            {
                column.Property ^= ColumnProperty.Null;
                column.Property |= ColumnProperty.NotNull;
                return this;
            }
        }

        public TableColumn<T> Having(ColumnProperty property)
        {
            column.Property = property;
            return this;
        }

        public TableColumn<T> WithColumn
        {
            get { return new TableColumn<T>(table); }
        }

        public void Add()
        {
            table.Run();
        }
    }
}