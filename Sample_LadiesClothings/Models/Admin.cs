using System.ComponentModel.DataAnnotations;

namespace Sample_LadiesClothings.Models
{
    public class Admin
    {
        [Key]
        public int? Admin_Id { get; set; } = 0;
        public string? Admin_Name { get; set; } = string.Empty;
        public string? Admin_Email { get; set; } = string.Empty;
        public string? Admin_Password { get; set; } = string.Empty;
        public string? Admin_Image { get; set; } = string.Empty;

    }
}
