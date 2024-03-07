using System.ComponentModel.DataAnnotations;

namespace Final_Back.Models
{
    public class ContactInfo
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Email { get; set; }

    }
}
