using System.Collections.Generic;
using System.Threading.Tasks;

namespace FreeResponse.EntityFrameworkCore.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAll();
        Task<TEntity> Get(int id);
        bool Save();
        void Add(TEntity item);
    }
}

