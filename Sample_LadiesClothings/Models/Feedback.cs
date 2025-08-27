using System.ComponentModel.DataAnnotations;

namespace Sample_LadiesClothings.Models
{
    public class Feedback
    {
        [Key]
        public int Feedback_Id { get; set; }
        public string User_Name { get; set; }
        public string User_Message { get; set; }

    }
}
