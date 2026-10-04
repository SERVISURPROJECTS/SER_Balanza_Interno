using Microsoft.EntityFrameworkCore;

namespace SER_Balanza_Interno.Data
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DbSet<T> _set;

        public Repository(AppDbContext context)
        {
            _set = context.Set<T>();
        }

        public IQueryable<T> Query() => _set.AsQueryable();

        /// <summary>
        /// Incluye tanto lo persistido como las entidades ya agregadas al contexto
        /// pero aun no guardadas (necesario para calcular "siguiente Id" de forma
        /// consistente cuando varias entidades nuevas se crean en la misma transaccion).
        /// </summary>
        public List<T> GetAll()
        {
            var almacenadas = _set.ToList();
            var pendientes = _set.Local.Where(e => !almacenadas.Contains(e));
            return almacenadas.Concat(pendientes).ToList();
        }

        public T? Find(params object?[] keyValues) => _set.Find(keyValues);

        public void Add(T entity) => _set.Add(entity);

        public void Remove(T entity) => _set.Remove(entity);
    }
}
