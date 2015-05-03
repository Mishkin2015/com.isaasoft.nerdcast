using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace Isaasoft.Core.Pagination
{
    public class PageRules<T>
        where T : class
    {
        private bool paging = true;

        public PageRules(IQueryable<T> source)
        {
            Source = source;
        }

        public PageRules(DbQuery<T> source, params string[] navigators)
        {
            var query = source as DbQuery<T>;

            foreach (var prop in navigators)
            {
                query = query.Include(prop);
            }

            Source = query.AsQueryable();
        }

        /// <summary>
        /// Get or set paging is active. Default true.
        /// </summary>
        public bool Paging
        {
            get { return paging; }
            set { paging = value; }
        }

        public int StartIndex { get; set; }

        public int TotalItems { get; set; }

        public IQueryable<T> Source { get; set; }

        public IEnumerable<ExpressionBlock> Blocks { get; set; }

        public IEnumerable<ExpressionOrderBy> OrderBy { get; set; }

        public IEnumerable<T> Invoke()
        {
            return InvokeQueryable().ToList();
        }

        public IQueryable<T> InvokeQueryable()
        {
            var queryable = default(IQueryable<T>);

            if (Paging)
            {
                if (Source == null)
                {
                    throw new ArgumentException("Source");
                }

                if (StartIndex <= -1)
                {
                    throw new ArgumentException("StartIndex");
                }

                if (TotalItems <= 0)
                {
                    throw new ArgumentException("TotalItems");
                }

                if (OrderBy == null || !OrderBy.Any())
                {
                    throw new ArgumentException("OrderBy");
                }

                queryable = Source.Where(Blocks)
                    .OrderBy(OrderBy)
                    .Skip(StartIndex)
                    .Take(TotalItems);
            }
            else
            {
                queryable = Source.Where(Blocks);

                if (OrderBy != null)
                {
                    queryable = queryable.OrderBy(OrderBy);
                }
            }

            return queryable;
        }

        public long InvokeCount()
        {
            if (Source == null)
            {
                throw new InvalidOperationException();
            }

            return Source.Where(Blocks).Count();
        }
    }
}
