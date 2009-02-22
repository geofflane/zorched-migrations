using System;
using System.Text;
using Zorched.Migrations.Framework.Schema;

namespace Zorched.Migrations.SqlServer.Schema
{
    public class SqlAddCheckConstraintOperation : BaseSchemaOperation, IAddCheckConstraintOperation
    {
        public string ConstraintDefinition { get; set; }
        public string ConstraintName { get; set; }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(TableName))
                throw new ArgumentException("TableName must be set.");

            if (string.IsNullOrEmpty(ConstraintDefinition))
                throw new ArgumentException("ConstraintDefinition must be set.");

            if (string.IsNullOrEmpty(ConstraintName))
                throw new ArgumentException("ConstraintName must be set.");

            var sb = new StringBuilder("ALTER TABLE ");
            AddTableInfo(sb, SchemaName, TableName);

            sb.Append(" WITH CHECK ADD CONSTRAINT ").AppendFormat(QUOTE_FORMAT, ConstraintName);
            sb.Append(" CHECK(").Append(ConstraintDefinition).Append(")");

            return sb.ToString();
        }
    }
}