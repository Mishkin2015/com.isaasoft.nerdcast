namespace Isaasoft.Core.Pagination
{
    public class PagingResult<TSource>
    {
        public long StartRowIndex { get; set; }

        public long ItemsPerPage { get; set; }

        public TSource Source { get; set; }

        public long TotalItemCount { get; set; }
    }
}
