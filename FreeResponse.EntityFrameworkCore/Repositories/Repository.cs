using FreeResponse.EntityFrameworkCore.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FreeResponse.EntityFrameworkCore.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        private readonly FreeResponseDbContext _db;
        public Repository(FreeResponseDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<TEntity>> GetAll()
        {
            return await Task.FromResult(_db.Set<TEntity>());
        }

        public async Task<TEntity> Get(int id)
        {
            var entity = await Task.FromResult(_db.Set<TEntity>().Find(id));
            return entity;
        }

        public bool Save()
        {
            return _db.SaveChanges() >= 0;
        }

        public void Add(TEntity item)
        {
            _db.Add(item);
        }
    }
}

