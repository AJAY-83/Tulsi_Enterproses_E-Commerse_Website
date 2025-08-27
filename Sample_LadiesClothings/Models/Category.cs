using System.ComponentModel.DataAnnotations;

namespace Sample_LadiesClothings.Models
{
    public class Category
    {
        [Key]
        public int Category_Id { get; set; }
        public string Category_Name { get; set; } = string.Empty;

        public List<Product>? products { get; set; }
    }
}
