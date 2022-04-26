using System.Linq;
using System.Threading.Tasks;

namespace MyWebForum.Data.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class, new()
    {
        public IQueryable<TEntity> GetAll();

        public Task<TEntity> AddAsync(TEntity entity);

        public Task<TEntity> UpdateAsync(TEntity entity);
    }
}
