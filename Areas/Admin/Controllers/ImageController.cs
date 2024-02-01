using Final_Back.DAL;
using Final_Back.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Final_Back.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ImageController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ImageController(AppDbContext appDbContext, IWebHostEnvironment webHostEnvironment)
        {
            _dbContext = appDbContext;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            List<ImgContainer> Images = await _dbContext.ImgContainer.ToListAsync();

            return View(Images);
        }
        #region Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ImgContainer Image)
        {
            if (!ModelState.IsValid) return View(Image);
            if (!Image.PhotoFile.ContentType.Contains("image/"))
            {
                ModelState.AddModelError("PhotoFile", "The file must be in Image format.");
            }
            if(Image.PhotoFile.Length / 1024 > 60)
            {
                ModelState.AddModelError("PhotoFile", "The size of the image should not exceed 60 MB.");
            }
            var filename = $"{Guid.NewGuid()}_{Image.PhotoFile.FileName}";
            var path = Path.Combine(_webHostEnvironment.WebRootPath, "assets/img", filename);

            using(FileStream fileStream = new FileStream(path, FileMode.Create,FileAccess.ReadWrite))
            {
                await Image.PhotoFile.CopyToAsync(fileStream);
            }
            Image.Image = filename;
            await _dbContext.ImgContainer.AddAsync(Image);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));    
        }
        #endregion
        #region Update
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            if (id == null) return BadRequest();
            
            ImgContainer Image = await _dbContext.ImgContainer.FindAsync(id);
            if (Image == null) return NotFound();
            
            return View(Image);
        }
        [HttpPost]
        public async Task<IActionResult> Update(ImgContainer Image, int id)
        {

            if (id == null) return BadRequest();
            

            Image = await _dbContext.ImgContainer.FindAsync(id);

            if (Image == null) return NotFound();
            

            if (!ModelState.IsValid) return View(Image);
       
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }
        #endregion
        #region Delete
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var dbImage = await _dbContext.ImgContainer.FindAsync(id);
            if (dbImage == null) return NotFound();
            var path = Path.Combine(_webHostEnvironment.WebRootPath, "assets/img", dbImage.Image);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            _dbContext.ImgContainer.Remove(dbImage);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        #endregion
    }
}
