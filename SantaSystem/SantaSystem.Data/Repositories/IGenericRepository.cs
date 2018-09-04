using Microsoft.AspNet.Identity.EntityFramework;
using SantaSystem.Models.DomainModels;
using System.Linq;

namespace SantaSystem.Data.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        IdentityDbContext<User> GetDbContext();

        IQueryable<T> GetAll();

        void Add(T item);

        void Remove(T item);
    }
}
