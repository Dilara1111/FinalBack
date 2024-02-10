using System.ComponentModel.DataAnnotations;

namespace Final_Back.Areas.Admin.ViewModels.Customer
{
    public class CustomerUpdateVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "The name must be written"), MinLength(3, ErrorMessage = "Minimum 3 symbols")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Commentare must be written"), MinLength(20, ErrorMessage = "Minimum 20 symbols")]
        public string Description { get; set; }
        public string? FilePath { get; set; }
        public IFormFile? Photo { get; set; }
    }
}
