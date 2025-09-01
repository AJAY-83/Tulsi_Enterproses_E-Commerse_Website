namespace Sample_LadiesClothings.Models
{
    public class CustomerListViewModel
    {
        public List<Customer> Customers { get; set; } = new List<Customer>();

        // Search & Pagination
        public string? Search { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalRecords { get; set; }
        public int TotalPages => (int)Math.Ceiling((decimal)TotalRecords / PageSize);
    }

}
