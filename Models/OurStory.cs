using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Final_Back.Models
{
    public class OurStory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Description1 { get; set; }
        public string Description2 { get; set; }
        public string? FilePath { get; set; }
        [Required(ErrorMessage = "The Photo must be download")]
        [NotMapped]
        public IFormFile Photo { get; set; }
    }
}
