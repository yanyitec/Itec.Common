namespace Itec.ORM
{
    public interface IORMContext
    {
        string ConnectionString { get; }
        string Prefix { get; }
    }
}