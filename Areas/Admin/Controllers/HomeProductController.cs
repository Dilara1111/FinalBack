using Final_Back.DAL;
using Final_Back.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Final_Back.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeProductController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public HomeProductController(AppDbContext appDbContext, IWebHostEnvironment webHostEnvironment)
        {
            _dbContext = appDbContext;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            List<HomeProducts> homeProduct = await _dbContext.HomeProducts.ToListAsync();

            return View(homeProduct);
        }
        #region Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(HomeProducts homeProduct)
        {
            if (!ModelState.IsValid) return View(homeProduct);
            bool isExit = await _dbContext.HomeProducts
           .AnyAsync(x => x.Name.ToLower().Trim() == homeProduct.Name.ToLower().Trim());
            if (isExit)
            {
                ModelState.AddModelError("Name", "Product is already available");
                return View(homeProduct);
            }
            if (!homeProduct.Photo.ContentType.Contains("image/"))
            {
                ModelState.AddModelError("Photo", "The file must be in Image format.");
            }
            if (homeProduct.Photo.Length / 1024 > 60)
            {
                ModelState.AddModelError("Photo", "The size of the image should not exceed 60 MB.");
            }
            var filename = $"{Guid.NewGuid()}_{homeProduct.Photo.FileName}";
            var path = Path.Combine(_webHostEnvironment.WebRootPath, "assets/img", filename);

            using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
            {
                await homeProduct.Photo.CopyToAsync(fileStream);
            }
            homeProduct.FilePath = filename;
            await _dbContext.HomeProducts.AddAsync(homeProduct);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            
        }
        #endregion
        #region Update
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            if (id == null) return BadRequest();            
            HomeProducts homeProduct = await _dbContext.HomeProducts.FindAsync(id);
            if (homeProduct == null) return NotFound();
            return View(homeProduct);
        }
        [HttpPost]
        public async Task<IActionResult> Update(HomeProducts homeProduct, int id)
        {
            if (id == null) return BadRequest();            
            HomeProducts? dbProduct = await _dbContext.HomeProducts.FindAsync(id);
            if (dbProduct == null) return NotFound();
            if (!ModelState.IsValid)
            {
                return View(homeProduct);
            }
            bool isExist = await _dbContext.Products
           .AnyAsync(x => x.Name.ToLower().Trim() == homeProduct.Name.ToLower().Trim() && x.Id != id);

            if (isExist)
            {
                ModelState.AddModelError("Name", "This name already is exist");
                return View();
            }
            dbProduct.Name = homeProduct.Name;
            dbProduct.Description = homeProduct.Description;
            dbProduct.Price = homeProduct.Price;

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        #endregion
        #region Delete
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var dbProduct = await _dbContext.HomeProducts.FindAsync(id);
            if (dbProduct == null) return NotFound();
            var path = Path.Combine(_webHostEnvironment.WebRootPath, "assets/img", dbProduct.FilePath);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            _dbContext.HomeProducts.Remove(dbProduct);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        #endregion
    }
}
