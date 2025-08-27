namespace Sample_LadiesClothings.Models
{
    public class CartItem
    {
        public int Product_Id { get; set; }
        public string Product_Name { get; set; } = string.Empty;
        public decimal Product_Price { get; set; }
        public int Quantity { get; set; }
        public string? Product_Image { get; set; }
        public decimal LineTotal => Product_Price * Quantity;
    }
}
