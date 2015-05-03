using System.Collections.Generic;

namespace Isaasoft.Core.Pagination
{
    public class PaginationParameters
    {
        public int StartRowIndex { get; set; }

        public int ItemsPerPage { get; set; }

        public IEnumerable<ExpressionBlock> Blocks { get; set; }

        public IEnumerable<ExpressionOrderBy> OrderBy { get; set; }
    }
}
