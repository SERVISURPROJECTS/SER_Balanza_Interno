namespace SER_Balanza_Interno.Data
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> Query();
        List<T> GetAll();
        T? Find(params object?[] keyValues);
        void Add(T entity);
        void Remove(T entity);
    }
}
