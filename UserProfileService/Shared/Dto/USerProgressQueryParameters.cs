namespace UserProfileService.Shared.Dto
{
    public class USerProgressQueryParameters
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public decimal? SearchWeight { get; set; } = null;       
        //  public string? CategoryName { get; set; }  
        public DateOnly? Date { get; set; }= null;
    }
}
