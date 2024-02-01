using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Final_Back.Models
{
    public class AboutComponent
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="The name must be written")]
        public string Name { get; set; }
        [Required(ErrorMessage ="The Title must be written")]
        public string Title { get; set; }
        [Required(ErrorMessage ="The Description must be written")]
        public string Description { get; set; }
        public string? FilePath { get; set; }
        [Required(ErrorMessage ="The photo must be download")]
        [NotMapped]
        public IFormFile Photo { get; set; }
    }
}
