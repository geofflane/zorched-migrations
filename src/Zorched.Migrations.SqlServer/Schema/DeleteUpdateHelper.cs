using System.Text;
using Zorched.Migrations.Framework;

namespace Zorched.Migrations.SqlServer.Schema
{
    public class DeleteUpdateHelper
    {
        public void AddOnDeleteIfNeeded(StringBuilder sb, ConstraintProperty prop)
        {
            if (prop.HasOnDelete())
            {
                sb.Append(" ON DELETE ").Append(UpdateDeleteValue(prop));
            }
        }

        public void AddOnUpdateIfNeeded(StringBuilder sb, ConstraintProperty prop)
        {
            if (prop.HasOnUpdate())
            {
                sb.Append(" ON UPDATE ").Append(UpdateDeleteValue(prop));
            }
        }

        public string UpdateDeleteValue(ConstraintProperty prop)
        {
            switch (prop)
            {
                case ConstraintProperty.CascadeOnDelete:
                case ConstraintProperty.CascadeOnUpdate:
                    return "CASCADE";
                case ConstraintProperty.NullOnDelete:
                case ConstraintProperty.NullOnUpdate:
                    return "SET NULL";
                case ConstraintProperty.DefaultOnDelete:
                case ConstraintProperty.DefaultOnUpdate:
                    return "SET DEFAULT";
            }
            return null;
        }
    }
}