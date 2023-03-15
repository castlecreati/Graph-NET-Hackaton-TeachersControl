using System.ComponentModel.DataAnnotations;

namespace TeachersControl.Data
{
    public class Assignment
    {
        public string Id { get; set; }
        [Required]
        public string Teacher { get; set; }
        [Required]
        public string Subject { get; set; }
        [Required]
        public string Section { get; set; }
    }
}
