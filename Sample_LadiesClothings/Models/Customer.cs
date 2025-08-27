using System.ComponentModel.DataAnnotations;

namespace Sample_LadiesClothings.Models
{
    public class Customer
    {
        //[Key]
        //public int? Customer_Id { get; set; } = 0;
        //public string? Customer_Name { get; set; }=string.Empty;
        //public string? Customer_Email { get; set; } = string.Empty;
        //public string? Customer_Password { get; set; } = string.Empty;
        //public string? Customer_Image { get; set; } = string.Empty;
        //public string? Customer_Country { get; set; } = string.Empty;
        //public string? Customer_City { get; set; } = string.Empty;
        //public string? Customer_Address { get; set; } = string.Empty;
        //public string? Customer_Phone { get; set; } = string.Empty;
        //public string? Customer_Gender { get; set; } = string.Empty;
        [Key]
        public int Customer_Id { get; set; }
        [Required, MaxLength(80)]
        public string Customer_Name { get; set; } = string.Empty;
        [Required, EmailAddress, MaxLength(120)]
        public string Customer_Email { get; set; } = string.Empty;

        // store hashed password
        [Required]
        public string Customer_PasswordHash { get; set; } = string.Empty;

        public string? Customer_City { get; set; }
        public string? Customer_Country { get; set; }
        public string? Customer_Address { get; set; }
        public string? Customer_Gender { get; set; }
        public string? Customer_Phone { get; set; }
        public string? Customer_Image { get; set; }







    }
}
