using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace Isaasoft.Core.Pagination
{
    public class PaginationRules<T>
        where T : class
    {
        public PaginationRules(IQueryable<T> source)
        {
            Source = source;
        }

        public PaginationRules(DbQuery<T> source, params string[] navigators)
        {
            var query = source as DbQuery<T>;

            foreach (var prop in navigators)
            {
                query = query.Include(prop);
            }

            Source = query.AsQueryable();
        }

        public int StartRowIndex { get; set; }

        public int ItemsPerPage { get; set; }

        public IQueryable<T> Source { get; set; }

        public IEnumerable<ExpressionBlock> Blocks { get; set; }

        public IEnumerable<ExpressionOrderBy> OrderBy { get; set; }

        public IEnumerable<T> Invoke()
        {
            return InvokeQueryable().ToArray();
        }

        public IQueryable<T> InvokeQueryable()
        {
            if (Source == null) throw new ArgumentException("Source");
            if (StartRowIndex <= -1) throw new ArgumentException("FirstItemOnPage");
            if (ItemsPerPage <= 0) throw new ArgumentException("ItemCountOnPage");
            if (OrderBy == null || !OrderBy.Any()) throw new ArgumentException("OrderBy");

            return Source.Where(Blocks)
                .OrderBy(OrderBy)
                .Skip(StartRowIndex)
                .Take(ItemsPerPage);
        }

        public long InvokeCount()
        {
            if (Source == null) throw new InvalidOperationException();

            return Source.Where(Blocks).Count();
        }
    }
}
