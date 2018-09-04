using System.Linq;

namespace SantaSystem.Data.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> GetAll();

        void Add(T item);
    }
}
