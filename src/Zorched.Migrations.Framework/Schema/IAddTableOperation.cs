using System.Collections.Generic;
using System.Data;

namespace Zorched.Migrations.Framework.Schema
{
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