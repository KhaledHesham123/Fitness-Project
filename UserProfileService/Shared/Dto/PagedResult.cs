namespace UserProfileService.Shared.Dto
{
    public class PagedResult<T>
    {
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int Totalpage => (int)Math.Ceiling(TotalCount / (double)PageSize);
        public IEnumerable<T> Item { get; set; } = new HashSet<T>();
    }
}
