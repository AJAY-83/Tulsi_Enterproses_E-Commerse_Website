using System.ComponentModel.DataAnnotations;

namespace Sample_LadiesClothings.Models
{
    public class FAQ
    {
        [Key]
        public int FAQ_Id { get; set; }
        public string? FAQ_Question { get; set; }
        public string? FAQ_Answer { get; set; }
    }
}
