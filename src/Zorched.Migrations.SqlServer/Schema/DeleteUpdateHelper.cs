using System;
using System.Text;
using Zorched.Migrations.Framework.Schema;

public class DeleteUpdateHelper
{
    public void AddOnDeleteIfNeeded(StringBuilder sb, ConstraintProperty prop)
    {
        if (prop.HasDelete()) 
        {
            sb.Append(" ON DELETE ").Append(UpdateDeleteValue);
        }
    }
    
    public void AddOnUpdateIfNeeded(StringBuilder sb, ConstraintProperty prop)
    {
        if (prop.HasUpdate()) 
        {
            sb.Append(" ON UPDATE ").Append(UpdateDeleteValue);
        }
    }
    
    public string UpdateDeleteValue()
    {
        switch(Property)
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