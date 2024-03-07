using Final_Back.Areas.Admin.ViewModels.Images;
using Final_Back.DAL;
using Final_Back.Helpers;
using Final_Back.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Final_Back.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin")]
    public class ImageController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IFileService _fileService;
        public ImageController(AppDbContext appDbContext, IWebHostEnvironment webHostEnvironment,
            IFileService fileService)
        {
            _dbContext = appDbContext;
            _webHostEnvironment = webHostEnvironment;
            _fileService = fileService;
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
        public async Task<IActionResult> Create(ImgContainer model)
        {
            if (!ModelState.IsValid) return View(model);
            if (model.PhotoFile != null)
            {
                if (!_fileService.IsImage(model.PhotoFile))
                {
                    ModelState.AddModelError("PhotoFile", "The file must be in Image format.");
                    return View(model);
                }
                int maxSize = 1024;
                if (!_fileService.CheckSize(model.PhotoFile, maxSize))
                {
                    ModelState.AddModelError("PhotoFile", $"The size of the image should not exceed {maxSize} KB.");
                    return View(model);
                }

                var filename = await _fileService.UploadAsync(model.PhotoFile);
                model.Image = filename;
            }
            await _dbContext.ImgContainer.AddAsync(model);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        #endregion
        #region Update
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var Image = await _dbContext.ImgContainer.FindAsync(id);
            if (Image == null)
            {
                return BadRequest();
            }
            var model = new ImageUpdateVM
            {
                Id = Image.Id,
                Image = Image.Image,
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Update(ImageUpdateVM model, int id)
        {

            if (id != model.Id) return BadRequest();
            if (!ModelState.IsValid) return View(model);

            var dbImage = await _dbContext.ImgContainer.FindAsync(id);
            dbImage.Image = model.Image;
            

            if (model.PhotoFile != null)
            {

                if (!_fileService.IsImage(model.PhotoFile))
                {
                    ModelState.AddModelError("PhotoFile", "The file must be in Image format.");
                    return View(model) ;
                }
                int maxSize = 60;
                if (!_fileService.CheckSize(model.PhotoFile, maxSize))
                {
                    ModelState.AddModelError("PhotoFile", $"The size of the image should not exceed {maxSize} KB");
                    //Image.Image = dbImage.Image; //submit eleyende shekil silinmesin deye 
                    return View(model);
                }
                string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets/img"); //, dbImage.Image);
                _fileService.Delete(path);
                var filename = await _fileService.UploadAsync(model.PhotoFile);
                dbImage.Image = filename;   
            }

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
