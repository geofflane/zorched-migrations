using System;
using System.Text;
using Zorched.Migrations.Framework.Schema;

namespace Zorched.Migrations.SqlServer.Schema
{
    public class SqlDropConstraintOperation : BaseSchemaOperation, IDropConstraintOperation
    {
        public string ConstraintName { get; set; }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(TableName))
                throw new ArgumentException("TableName must be set.");

            if (string.IsNullOrEmpty(ConstraintName))
                throw new ArgumentException("ConstraintName must be set.");

            var sb = new StringBuilder("ALTER TABLE ");
            AddTableInfo(sb, SchemaName, TableName);
            sb.Append(" DROP CONSTRAINT ").AppendFormat(QUOTE_FORMAT, ConstraintName);

            return sb.ToString();
        }
    }
}