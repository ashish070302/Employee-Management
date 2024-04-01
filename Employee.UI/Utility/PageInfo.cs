namespace Employee.UI.Utility
{
    public class PageInfo
    {
        public int PageNumber { get; set; }
        public int PageSize {  get; set; }
        public int TotalItems { get; set; }
        public int TotalPages => (int)Math.Ceiling((decimal) TotalItems / PageSize);
        public bool HasPrevious => (PageNumber > 1);
        public bool HasNext => (PageNumber < TotalPages);
    }
}
