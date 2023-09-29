using System.Linq.Expressions;

namespace ProvaPub.Repository
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        IEnumerable<TEntity> Get(int page);
        bool HasNext(int page);
    }
}
