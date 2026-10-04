namespace SER_Balanza_Interno.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private readonly Dictionary<Type, object> _repositories = new();

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public IRepository<T> Repository<T>() where T : class
        {
            var type = typeof(T);
            if (!_repositories.TryGetValue(type, out var repository))
            {
                repository = new Repository<T>(_context);
                _repositories[type] = repository;
            }

            return (IRepository<T>)repository;
        }

        public int SaveChanges() => _context.SaveChanges();

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }

    public static class UnitOfWorkFactory
    {
        public static IUnitOfWork Create() => new UnitOfWork(AppDbContextFactory.Create());
    }
}
