using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using Isaasoft.Core.Pagination;
using System.Data.Entity.Core.Objects;
using System.Threading;
using System.Threading.Tasks;

namespace Isaasoft.Core.Entity
{
    public class BaseRules<TDbContext> : IDisposable
        where TDbContext : DbContext, new()
    {
        protected bool disposed = false;

        public BaseRules(TDbContext dbContext)
        {
            dbContext.Configuration.LazyLoadingEnabled = false;

            DbContext = dbContext;
        }

        public TDbContext DbContext { get; protected set; }

        public virtual void SetContext(TDbContext context)
        {
            this.DbContext = context;
        }

        public IQueryable<TEntity> GetQueryable<TEntity>(params string[] navigators)
            where TEntity : class
        {
            var query = DbContext.Set<TEntity>() as DbQuery<TEntity>;

            foreach (var prop in navigators)
            {
                query = query.Include(prop);
            }

            return query.AsQueryable();
        }

        protected PaginationResult<IQueryable<TEntity>> GetPagination<TEntity>(PaginationParameters parameters, params string[] navigators)
             where TEntity : class
        {
            var response = new PaginationResult<IQueryable<TEntity>>();

            var builder = new PaginationRules<TEntity>(DbContext.Set<TEntity>(), navigators)
            {
                StartRowIndex = parameters.StartRowIndex,
                ItemsPerPage = parameters.ItemsPerPage,
                Blocks = parameters.Blocks,
                OrderBy = parameters.OrderBy
            };

            response.StartRowIndex = parameters.StartRowIndex;
            response.ItemsPerPage = parameters.ItemsPerPage;
            response.TotalItemCount = builder.InvokeCount();
            response.Source = builder.InvokeQueryable();

            return response;
        }

        public void SaveChanges(int? timeout)
        {
            var oldTimeout = this.DbContext.Database.CommandTimeout;

            this.DbContext.Database.CommandTimeout = timeout;
            this.DbContext.SaveChanges();

            this.DbContext.Database.CommandTimeout = oldTimeout;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                disposed = true;

                if (disposing)
                {
                }

                DbContext.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
