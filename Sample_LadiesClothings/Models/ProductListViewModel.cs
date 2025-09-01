namespace Sample_LadiesClothings.Models
{
    public class ProductListViewModel
    {
        public List<Product> Products { get; set; } = new();
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalRecords { get; set; } = 0;
        public string Search { get; set; } = string.Empty;
        public int TotalPages => (int)Math.Ceiling((double)TotalRecords / Math.Max(1, PageSize));
    }

}
