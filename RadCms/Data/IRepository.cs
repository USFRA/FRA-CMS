using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadCms.Data
{
    public interface IRepository<TEntity> : IDisposable where TEntity : IEntity
    {
        IQueryable<TEntity> GetAll();
        TEntity Get(int id);
        void Delete(TEntity entity);
        void DeleteAll(IEnumerable<TEntity> entities);
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Save();
    }
}
