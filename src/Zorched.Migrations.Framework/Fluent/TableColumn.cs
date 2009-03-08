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