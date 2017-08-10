using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
namespace RadCms.Data
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        private IDbContext _context;
        public Repository(IDbContext context)
        {
            _context = context;
        }
        private IDbSet<TEntity> DbSet
        {
            get
            {
                return _context.Set<TEntity>();
            }
        }
        public IQueryable<TEntity> GetAll()
        {
            return DbSet.AsQueryable();
        }
        public TEntity Get(int id)
        {
            return DbSet.Find(id);
        }
        public void Delete(TEntity entity)
        {
            DbSet.Remove(entity);
        }
        public void DeleteAll(IEnumerable<TEntity> entities)
        {
            foreach(TEntity entity in entities)
            {
                DbSet.Remove(entity);
            }
        }
        public void Add(TEntity entity)
        {
            DbSet.Add(entity);
        }

        public void Update(TEntity entity)
        {
            ((DbContext)_context).Entry<TEntity>(entity).State = EntityState.Modified;
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_context != null)
                {
                    _context.Dispose();
                    _context = null;
                }
            }
        }
    }
}
