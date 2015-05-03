using System.Collections.Generic;

namespace Isaasoft.Core.Pagination
{
    public class PagingParameters
    {
        public int FirstItemOnPage { get; set; }

        public int ItemCountOnPage { get; set; }

        public IEnumerable<ExpressionBlock> Blocks { get; set; }

        public IEnumerable<ExpressionOrderBy> OrderBy { get; set; }
    }
}
