namespace Zorched.Migrations.Framework.Fluent
{
    public interface IColumnUser
    {
        void SetColumn(Column c);
        void Run();
    }
}