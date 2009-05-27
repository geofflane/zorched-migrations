namespace Zorched.Migrations.Framework
{
    /// <summary>
    /// Common interface for Up and Down migrations that allow them to be ordered,
    /// if desired, within a single Migration class.
    /// </summary>
    public interface IMigrationDirection
    {
        int Order { get; set; }
    }
}