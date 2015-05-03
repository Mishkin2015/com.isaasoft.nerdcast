using Isaasoft.Core.Pagination;
using System.Data.Entity;
using System.Linq;

namespace Isaasoft.Core.Entity
{
    public class BaseRules<TDbContext, TEntity> : BaseRules<TDbContext>
        where TDbContext : DbContext, new()
        where TEntity : class
    {
        public BaseRules(TDbContext entities)
            : base(entities)
        {
        }

        public IQueryable<TEntity> GetQueryable(params string[] navigators)
        {
            return this.GetQueryable<TEntity>(navigators);
        }

        public PaginationResult<IQueryable<TEntity>> GetPagination(PaginationParameters parameters, params string[] navigators)
        {
            return this.GetPagination<TEntity>(parameters, navigators);
        }
    }
}
