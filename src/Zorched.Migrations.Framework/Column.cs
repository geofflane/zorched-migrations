using System.Data;

namespace Zorched.Migrations.Framework
{
    public class Column
    {

        public string Name { get; set; }
        
        public DbType DbType { get; set; }
        
        public object DefaultValue { get; set; }

        public int Size { get; set; }
        
        public ColumnProperty Property { get; set; }
    }
}
