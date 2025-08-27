using System.ComponentModel.DataAnnotations;

namespace Sample_LadiesClothings.Models
{
    public class Cart
    {
        [Key]
        public int Card_Id { get; set; }
        public int Product_Id { get; set; }
        public int Customer_Id { get; set; } = 0;
        public int Product_Quantiry { get; set; }
        public int Status { get; set; } = 0;
    }
}
