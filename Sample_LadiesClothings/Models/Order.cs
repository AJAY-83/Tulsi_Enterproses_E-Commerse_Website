
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Sample_LadiesClothings.Models
{
    public class Order
    {
        [Key]
        public int Order_Id { get; set; }
        public int Customer_Id { get; set; }
        public DateTime Order_Date { get; set; } = DateTime.UtcNow;
        public string Status { get; set; } = "Pending";
        [Column(TypeName = "decimal(10,2)")]
        public decimal Total { get; set; }

        public List<OrderItem> Items { get; set; } = new();
    }

    public class OrderItem
    {
        [Key]
        public int OrderItem_Id { get; set; }
        public int Order_Id { get; set; }
        public int Product_Id { get; set; }
        public int Quantity { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }
    }
}
