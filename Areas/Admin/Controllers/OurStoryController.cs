using Final_Back.DAL;
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
        public OurStoryController(AppDbContext appDbContext,IWebHostEnvironment webHostEnvironment)
        {
            _dbContext = appDbContext;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            OurStory ourStory = await _dbContext.OurStory.FirstOrDefaultAsync();

            return View(ourStory);
        }
        #region Create
        public async Task<IActionResult> Create(OurStory ourStory)
        {
            if (!ModelState.IsValid) return View(ourStory);
           
            if (!ourStory.Photo.ContentType.Contains("image/"))
            {
                ModelState.AddModelError("Photo", "The file must be in Image format.");
                return View(ourStory);
            }
            if (ourStory.Photo.Length / 1024 > 60)
            {
                ModelState.AddModelError("Photo", "The size of the image should not exceed 60 MB.");
                return View(ourStory);
            }
            var filename = $"{Guid.NewGuid()}_{ourStory.Photo.FileName}";
            var path = Path.Combine(_webHostEnvironment.WebRootPath, "assets/img", filename);

            using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
            {
                await ourStory.Photo.CopyToAsync(fileStream);
            }
            ourStory.FilePath = filename;
            await _dbContext.OurStory.AddAsync(ourStory);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        #endregion
        #region Update
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            if (id == null)  return BadRequest();
            OurStory ourStory = await _dbContext.OurStory.FindAsync(id);
            if (ourStory == null)  return NotFound();
            return View(ourStory);
        }
        [HttpPost]
        public async Task<IActionResult> Update(OurStory ourStory, int id)
        {
            if (id == null)  return BadRequest();
            ourStory = await _dbContext.OurStory.FindAsync(id);
            if (ourStory == null) return NotFound();
            if (!ModelState.IsValid)  return View(ourStory);
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
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            _dbContext.OurStory.Remove(dbOurStory);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        #endregion
    }
}

