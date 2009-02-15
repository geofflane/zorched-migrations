using System.Collections.Generic;
using System.Data;
using System.Text;
using Zorched.Migrations.Framework;
using Zorched.Migrations.Framework.Extensions;
using Zorched.Migrations.Framework.Schema;

namespace Zorched.Migrations.Providers.SQLServer.Schema
{
    class SqlAddTableOperation : IAddTableOperation
    {

        private const string QUOTE_FORMAT = "[{0}]";

        private const string PK_FORMAT = QUOTE_FORMAT + " ASC";
        private const string PK_BLOCK =
@"PRIMARY KEY CLUSTERED 
(
	{0}
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)";

        public SqlAddTableOperation()
        {
            SchemaName = "dbo";
            Columns = new List<Column>();
        }

        public string SchemaName { get; set; }
        public string TableName { get; set; }
        public IList<Column> Columns { get; set; }

        public void AddColumn(Column c)
        {
            Columns.Add(c);
        }

        public void AddColumn(string name, DbType type, int size, object defaultValue)
        {
            AddColumn(new Column {Name = name, DbType = type, Size = size, DefaultValue = defaultValue});
        }

        public void AddColumn(string name, DbType type, int size)
        {
            AddColumn(new Column { Name = name, DbType = type, Size = size });
        }

        public void Execute(IDbCommand command)
        {
            var sb = new StringBuilder("CREATE TABLE ");
            if (!string.IsNullOrEmpty(SchemaName))
                sb.AppendFormat(QUOTE_FORMAT, SchemaName).Append(".");

            sb.AppendFormat(QUOTE_FORMAT, TableName).AppendLine(" (");
            AddColumns(sb);
            sb.AppendLine(")");

        }

        private void AddColumns(StringBuilder sb)
        {
            var pks = new List<Column>();

            Columns.ForEach(
                c =>
                    {
                        sb.AppendFormat(QUOTE_FORMAT, c.Name);
                        sb.Append(" ").Append(DbTypeMap.GetTypeName(c.DbType, c.Size));
                        if (c.Property.Match(ColumnProperty.Identity))
                        {
                            sb.Append(" IDENTITY(1,1)");
                        }
                        sb.Append(c.Property.Match(ColumnProperty.Null) ? " NULL" : " NOT NULL");
                        if (null != c.DefaultValue)
                        {
                            sb.Append("DEFAULT ").Append(c.DefaultValue);
                        }
                        sb.AppendLine(",");

                        if (c.Property.Match(ColumnProperty.PrimaryKey))
                            pks.Add(c);
                    });

            if (0 != pks.Count)
            {
                var pksSb = new StringBuilder();
                pks.ForEach(c => pksSb.AppendFormat(PK_FORMAT, c.Name).AppendLine(","));
                pksSb.TrimEnd(',');
                sb.AppendFormat(PK_BLOCK, pksSb).AppendLine();
            }

            sb.TrimEnd(',');
        }
    }
}