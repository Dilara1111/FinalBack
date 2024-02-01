using Final_Back.DAL;
using Final_Back.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Final_Back.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OurStoryController : Controller
    {
        private readonly AppDbContext _dbContext;
        public OurStoryController(AppDbContext appDbContext)
        {
            _dbContext = appDbContext;
        }
        public async Task<IActionResult> Index()
        {
            OurStory ourStory = await _dbContext.OurStory.FirstOrDefaultAsync();

            return View(ourStory);
        }
        //#region Create
        //[HttpGet]
        //public async Task<IActionResult> Create()
        //{
        //    return View();
        //}
        //[HttpPost]
        //public async Task<IActionResult> Create(OurStory ourStory)
        //{
        //    if (!ModelState.IsValid) return View(ourStory);
        //    await _dbContext.OurStory.AddAsync(ourStory);
        //    await _dbContext.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}
        //#endregion
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
            _dbContext.OurStory.Remove(dbOurStory);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        #endregion
    }
}

