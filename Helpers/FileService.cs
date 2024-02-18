using Final_Back.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace Final_Back.Helpers
{
    public class FileService:IFileService 
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public FileService(IWebHostEnvironment webHostEnvironment) 
        { 
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<string> UploadAsync(IFormFile file)
        {
            var filename = $"{Guid.NewGuid()}_{file.FileName}";
            var path = Path.Combine(_webHostEnvironment.WebRootPath, "assets/img", filename);

            using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
            {
                await file.CopyToAsync(fileStream);
            }
            return filename;
        }
    
        public bool IsImage(IFormFile file)
        {
            return file.ContentType.Contains("img/");
        }
        public bool CheckSize(IFormFile file,int maxSize)
        {
            if(file.Length > maxSize)
            {
                return false;
            }
            return true;
        }
        public void Delete(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

    }

}
