namespace Zorched.Migrations.Framework
{
    public interface IMigrationDirection
    {
        int Order { get; set; }
    }
}