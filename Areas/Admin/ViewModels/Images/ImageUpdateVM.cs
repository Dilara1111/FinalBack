
namespace Final_Back.Areas.Admin.ViewModels.Images
{
    public class ImageUpdateVM
    {
        public int Id { get; set; }

        public string? Image { get; set; }
        public IFormFile? PhotoFile { get; set; }
    }
}
