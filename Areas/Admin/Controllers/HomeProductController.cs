using Final_Back.Areas.Admin.ViewModels.HomeProduct;
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
    public class HomeProductController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IFileService _fileService;

        public HomeProductController(AppDbContext appDbContext, IWebHostEnvironment webHostEnvironment
            , IFileService fileService)
        {
            _dbContext = appDbContext;
            _webHostEnvironment = webHostEnvironment;
            _fileService = fileService;
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
            if (homeProduct.Photo.ContentType.Contains("image/"))
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
            var Product = await _dbContext.Products.FindAsync(id);
            if (Product == null)
            {
                return BadRequest();
            }
            var model = new HomeProductUpdateVM
            {
                Id = Product.Id,
                Name = Product.Name,
                Description = Product.Description,
                FilePath = Product.FilePath,
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Update(HomeProductUpdateVM Product, int id)
        {

            if (id != Product.Id) return BadRequest();

            if (!ModelState.IsValid) return View(Product);
            var dbProduct = await _dbContext.Products.FindAsync(id);
            dbProduct.Name = Product.Name;
            dbProduct.Description = Product.Description;
            dbProduct.Price = Product.Price;
            if (dbProduct == null) return NotFound();

            bool isExist = await _dbContext.Products
            .AnyAsync(x => x.Name.ToLower().Trim() == Product.Name.ToLower().Trim() && x.Id != id);

            if (isExist)
            {
                ModelState.AddModelError("Name", "The title is already exist");
                return View();
            }
            if (!Product.Photo.ContentType.Contains("image/"))
            {
                ModelState.AddModelError("Photo", "The file must be in Image format.");
                return View(Product);
            }
            if (Product.Photo.Length / 1024 > 80)
            {
                ModelState.AddModelError("Photo", "The size of the image should not exceed 80 MB.");
                return View(Product);
            }

            if (Product.Photo != null)
            {
                string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets/img", dbProduct.FilePath);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                _dbContext.Products.Remove(dbProduct);
            }

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
