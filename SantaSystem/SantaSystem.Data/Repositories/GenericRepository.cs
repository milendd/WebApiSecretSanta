using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantaSystem.Data.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private IDbSet<T> dbSet;
        private SantaSystemDbContext dbContext;
        private IDbFactory dbFactory { get; set; }
        
        public GenericRepository(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
            this.dbSet = this.DbContext.Set<T>();
        }

        protected SantaSystemDbContext DbContext
        {
            get { return dbContext ?? (dbContext = dbFactory.GetContext()); }
        }

        private IDbSet<T> DbSet
        {
            get { return this.dbSet; }
        }

        public IQueryable<T> GetAll()
        {
            return DbContext.Set<T>().AsNoTracking();
        }

        public void Add(T item)
        {
            this.DbSet.Add(item);
            this.DbContext.SaveChanges();
        }
    }
}
