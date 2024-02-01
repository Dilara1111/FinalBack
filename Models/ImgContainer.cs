using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Final_Back.Models
{
    public class ImgContainer
    {
        public int Id { get; set; }
        
        public string? Image { get; set; }
        [Required(ErrorMessage = "The Photo must be download")]
        [NotMapped]
        public IFormFile PhotoFile { get; set; }

    }
}
