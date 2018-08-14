using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantaSystem.Data.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private SantaSystemDbContext dbContext;
        private IDbFactory dbFactory { get; set; }
        
        public GenericRepository(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        protected SantaSystemDbContext DbContext
        {
            get { return dbContext ?? (dbContext = dbFactory.GetContext()); }
        }

        public IQueryable<T> GetAll()
        {
            return DbContext.Set<T>().AsNoTracking();
        }
    }
}
