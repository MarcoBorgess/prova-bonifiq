using Microsoft.EntityFrameworkCore;
using ProvaPub.Models;
using System.Collections.Generic;

namespace ProvaPub.Repository
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        internal TestDbContext context;
        internal DbSet<TEntity> dbSet;

        public BaseRepository(TestDbContext context)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
        }

        public IEnumerable<TEntity> Get(int page)
        {
            IQueryable<TEntity> query = dbSet;
            
            if (page < 1)
                return query.ToList();

            return query.Skip((page - 1) * 10).Take(10).ToList();
        }

        public bool HasNext(int page)
        {
            if (page < 1)
                return false;

            IQueryable<TEntity> query = dbSet;

            return (query.Count() / 10) > page;
        }
    }
}
