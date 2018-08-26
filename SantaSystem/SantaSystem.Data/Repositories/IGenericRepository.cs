using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantaSystem.Data.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> GetAll();

        void Add(T item);
    }
}
