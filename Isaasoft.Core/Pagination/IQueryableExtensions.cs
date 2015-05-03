using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;

namespace Isaasoft.Core.Pagination
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> Where<T>(this IQueryable<T> source, IEnumerable<ExpressionBlock> blocks)
        {
            if (blocks == null || !blocks.Any())
            {
                return source;
            }

            var expressions = default(IEnumerable<ExpressionProperty>);

            return source.Where(
                ExpressionBlock.BuildExpressionString(blocks, out expressions), 
                expressions.Select(m => Helpers.ConvertValue<T>(m)).ToArray());
        }

        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, IEnumerable<ExpressionOrderBy> orderBy)
        {
            if (orderBy == null || !orderBy.Any())
            {
                throw new ArgumentNullException("ordination");
            }

            var orderByString = orderBy.Select(m => m.BuildExpressionString());

            return source.OrderBy(string.Join(", ", orderByString));
        }
    }
}
