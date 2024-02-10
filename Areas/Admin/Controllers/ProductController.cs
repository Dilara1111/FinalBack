using Final_Back.Areas.Admin.ViewModels.Product;
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
            var product = new Products()
            {
                Name = Product.Name,
                Description = Product.Description,
                Price = Product.Price,
                FilePath = Product.FilePath,
                Quantity = Product.Quantity,
            };
            await _dbContext.Products.AddAsync(Product);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        #endregion
        #region Update
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var Product = await _dbContext.Products.FindAsync(id);
            if (id == null)
            {
                return BadRequest();
            }
            var model = new ProductUpdateVM
            {
                Id = Product.Id,
                Name = Product.Name,
                Description = Product.Description,
                FilePath = Product.FilePath,
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Update(ProductUpdateVM Product, int id)
        {

            if (id == Product.Id) return BadRequest();

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
            
            if(Product.Photo != null)
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
