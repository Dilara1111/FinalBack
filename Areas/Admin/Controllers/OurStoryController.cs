using Final_Back.DAL;
using Final_Back.Helpers;
using Final_Back.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Final_Back.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin")]
    public class OurStoryController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IFileService _fileService;
        public OurStoryController(AppDbContext appDbContext,
            IWebHostEnvironment webHostEnvironment, IFileService fileService)
        {
            _dbContext = appDbContext;
            _webHostEnvironment = webHostEnvironment;
            _fileService = fileService;
        }
        public async Task<IActionResult> Index()
        {
            OurStory ourStory = await _dbContext.OurStory.FirstOrDefaultAsync();

            return View(ourStory);
        }
        #region Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(OurStory ourStory)
        {
            if (!ModelState.IsValid) return View(ourStory);
            if(ourStory.Photo != null)
            {
                if (!_fileService.IsImage(ourStory.Photo))
                {
                    ModelState.AddModelError("Photo", "The file must be in Image format.");
                    return View(ourStory);
                }
                int maxSize = 100;
                if (!_fileService.CheckSize(ourStory.Photo, maxSize))
                {
                    ModelState.AddModelError("Photo", $"The size of the image should not exceed {maxSize} KB.");
                    return View(ourStory);
                }
                var filename = await _fileService.UploadAsync(ourStory.Photo);
                ourStory.FilePath = filename;
            }
           
            await _dbContext.OurStory.AddAsync(ourStory);

            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        #endregion
        #region Update
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var ourStory = await _dbContext.OurStory.FirstOrDefaultAsync(x=> x.Id ==id);
            if (ourStory == null)  return NotFound();
            var model = new OurStory 
            { 
                Id = ourStory.Id,
                Photo = ourStory.Photo,
                Name = ourStory.Name,
                Description1 = ourStory.Description1,
                Description2 = ourStory.Description2,
                Title = ourStory.Title,
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Update(OurStory ourStory, int id)
        {
            if(!ModelState.IsValid) return View(ourStory);
            if (id != ourStory.Id)  return BadRequest();
            var dbourStory = await _dbContext.OurStory.FindAsync(id);
            dbourStory.Name = ourStory.Name;
            dbourStory.Title = ourStory.Title;
            dbourStory.Description1 = ourStory.Description1;
            dbourStory.Description2 = ourStory.Description2;
            if (ourStory.Photo != null)
            {
                if (!_fileService.IsImage(ourStory.Photo))
                {
                    ModelState.AddModelError("Photo", "The file must be in Image format.");
                    return View(ourStory);
                }
                int maxSize = 100;
                if (!_fileService.CheckSize(ourStory.Photo, maxSize))
                {
                    ModelState.AddModelError("Photo", $"The size of the image should not exceed {maxSize} MB");
                    //ourStory.FilePath = dbourStory.FilePath; //submit eleyende shekil silinmesin deye 
                    return View(ourStory);
                }
                string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets/img");
                _fileService.Delete(path);
                var filename = await _fileService.UploadAsync(ourStory.Photo);
                dbourStory.FilePath = filename;
            }
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        #endregion
        #region Delete
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var dbOurStory = await _dbContext.OurStory.FindAsync(id);
            if (dbOurStory == null) return NotFound();
            var path = Path.Combine(_webHostEnvironment.WebRootPath, "assets/img", dbOurStory.FilePath);
            _fileService.Delete(path);
            _dbContext.OurStory.Remove(dbOurStory);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        #endregion
    }
}

