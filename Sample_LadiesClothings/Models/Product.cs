using System.ComponentModel.DataAnnotations;

namespace Sample_LadiesClothings.Models
{
    public class Product
    {
        [Key]
        public int Product_Id { get; set; }
        public string Product_Name { get; set; } = string.Empty;
        public string Product_Price { get; set; } = string.Empty;
        public string Product_Description { get; set; } = string.Empty;
        public string Product_Image { get; set; } = string.Empty;
        public int Category_Id { get; set; }
        public bool IsFeatured { get; set; }   // new column

        public Category? category { get; set; } 
    }
}
