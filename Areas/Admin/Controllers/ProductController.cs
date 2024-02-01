using Final_Back.DAL;
using Final_Back.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Final_Back.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(AppDbContext appDbContext, IWebHostEnvironment webHostEnvironment)
        {
            _dbContext = appDbContext;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            List<Products> Products = await _dbContext.Products.ToListAsync();

            return View(Products);
        }
        #region Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Products Product)
        {
            if (!ModelState.IsValid) return View(Product);
            bool isExit = await _dbContext.Products
           .AnyAsync(x => x.Name.ToLower().Trim() == Product.Name.ToLower().Trim());
            if (isExit)
            {
                ModelState.AddModelError("Name", "Product is already available");
                return View(Product);
            }
            if (!Product.Photo.ContentType.Contains("image/"))
            {
                ModelState.AddModelError("Photo", "The file must be in Image format.");
                return View(Product);
            }
            if (Product.Photo.Length / 1024 > 60)
            {
                ModelState.AddModelError("Photo", "The size of the image should not exceed 60 MB.");
                return View(Product);
            }
            var filename = $"{Guid.NewGuid()}_{Product.Photo.FileName}";
            var path = Path.Combine(_webHostEnvironment.WebRootPath, "assets/img", filename);

            using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
            {
                await Product.Photo.CopyToAsync(fileStream);
            }
            Product.FilePath = filename;
            await _dbContext.Products.AddAsync(Product);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        #endregion
        #region Update
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Products Product = await _dbContext.Products.FindAsync(id);
            if (Product == null)
            {
                return NotFound();
            }
            return View(Product);
        }
        [HttpPost]
        public async Task<IActionResult> Update(Products Product, int id)
        {

            if (id == null) return BadRequest();

            Products? dbProduct = await _dbContext.Products.FindAsync(id);

            if (dbProduct == null) return NotFound();

            if (!ModelState.IsValid) return View(Product);
            
            bool isExist = await _dbContext.Products
            .AnyAsync(x => x.Name.ToLower().Trim() == Product.Name.ToLower().Trim() && x.Id != id);

            if (isExist)
            {
                ModelState.AddModelError("Name", "The title is already exist");
                return View();
            }
            dbProduct.Name = Product.Name;
            dbProduct.Description = Product.Description;
            dbProduct.Price = Product.Price;

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }
        #endregion
        #region Delete
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var dbProduct = await _dbContext.Products.FindAsync(id);
            if (dbProduct == null) return NotFound();
            var path = Path.Combine(_webHostEnvironment.WebRootPath, "assets/img", dbProduct.FilePath);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            _dbContext.Products.Remove(dbProduct);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        #endregion
    }
}
