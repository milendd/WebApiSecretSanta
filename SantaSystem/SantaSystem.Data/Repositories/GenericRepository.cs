using Microsoft.AspNet.Identity.EntityFramework;
using SantaSystem.Models.DomainModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace SantaSystem.Data.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly IDbSet<T> dbSet;
        private SantaSystemDbContext dbContext;
        private IDbFactory dbFactory { get; set; }
        
        public GenericRepository(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
            this.dbSet = this.DbContext.Set<T>();
        }

        public SantaSystemDbContext DbContext
        {
            get { return dbContext ?? (dbContext = dbFactory.GetContext()); }
        }

        private IDbSet<T> DbSet
        {
            get { return this.dbSet; }
        }

        public IdentityDbContext<User> GetDbContext()
        {
            return this.DbContext;
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

        public void Remove(T item)
        {
            this.DbSet.Attach(item);
            this.DbSet.Remove(item);
            this.DbContext.SaveChanges();
        }
    }
}
