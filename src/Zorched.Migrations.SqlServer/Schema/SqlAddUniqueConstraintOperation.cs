using System;
using System.Text;
using Zorched.Migrations.Framework.Schema;

namespace Zorched.Migrations.SqlServer.Schema
{
    public class SqlAddUniqueConstraintOperation : BaseSchemaOperation, IAddUniqueConstraintOperation
    {
        private DeleteUpdateHelper deleteUpdateHelper = new DeleteUpdateHelper();
        
        public string ColumnName { get; set; }
        public string ConstraintName { get; set; }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(TableName))
                throw new ArgumentException("TableName must be set.");

            if (string.IsNullOrEmpty(ColumnName))
                throw new ArgumentException("ColumnName must be set.");

            if (string.IsNullOrEmpty(ConstraintName))
                throw new ArgumentException("ConstraintName must be set.");

            var sb = new StringBuilder("ALTER TABLE ");
            AddTableInfo(sb, SchemaName, TableName);

            sb.Append(" WITH CHECK ADD CONSTRAINT ").AppendFormat(QUOTE_FORMAT, ConstraintName);
            sb.Append(" UNIQUE(").AppendFormat(QUOTE_FORMAT, ColumnName).Append(")");

            deleteUpdateHelper.AddOnDeleteIfNeeded(sb, Property);
            deleteUpdateHelper.AddOnUpdateIfNeeded(sb, Property);
            
            return sb.ToString();
        }
    }
}