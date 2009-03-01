using System;
using System.Text;
using Zorched.Migrations.Framework;
using Zorched.Migrations.Framework.Schema;

namespace Zorched.Migrations.SqlServer.Schema
{
    public class SqlChangeColumnOperation : BaseSchemaOperation, IChangeColumnOperation
    {
        public Column Column { get; set; }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(TableName))
                throw new ArgumentException("TableName must be set.");

            if (null == Column)
                throw new ArgumentException("Column must be set.");

            var sb = new StringBuilder("ALTER TABLE ");
            AddTableInfo(sb, SchemaName, TableName);
            sb.Append(" ALTER COLUMN ");
            AddColumnDefinition(sb, Column);

            return sb.ToString();
        }
    }
}