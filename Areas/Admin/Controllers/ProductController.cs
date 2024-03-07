using Final_Back.Areas.Admin.ViewModels.Product;
using Final_Back.DAL;
using Final_Back.Helpers;
using Final_Back.Migrations;
using Final_Back.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Final_Back.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IFileService _fileService;

        public ProductController(AppDbContext appDbContext,
            IWebHostEnvironment webHostEnvironment,
            IFileService fileService)
        {
            _dbContext = appDbContext;
            _webHostEnvironment = webHostEnvironment;
            _fileService = fileService;
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
            if (Product.Photo != null)
            {
                if (!_fileService.IsImage(Product.Photo))
                {
                    ModelState.AddModelError("Photo", "The file must be in Image format.");
                    return View(Product);
                }
                int maxSize = 100;
                if (!_fileService.CheckSize(Product.Photo, maxSize))
                {
                    ModelState.AddModelError("Photo", $"The size of the image should not exceed {maxSize} KB.");
                    return View(Product);
                }
                string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets/img");

                var filename = await _fileService.UploadAsync(Product.Photo);
                Product.FilePath = filename;
            }

            await _dbContext.Products.AddAsync(Product);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Update
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var Product = await _dbContext.Products
                .FirstOrDefaultAsync(p => p.Id == id);
            if (Product == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(Product);
            }

            var model = new ProductUpdateVM
            {
                Id = Product.Id,
                Name = Product.Name,
                Description = Product.Description,
                Price = Product.Price,
                Quantity = Product.Quantity,
                FilePath = Product.FilePath,
                Photo = Product.Photo,
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Update(ProductUpdateVM Product, int id)
        {

            if (!ModelState.IsValid) return View(Product);
            if (id != Product.Id) return BadRequest();

            var dbProduct = await _dbContext.Products.FindAsync(id);
            dbProduct.Name = Product.Name;
            dbProduct.Description = Product.Description;
            dbProduct.Price = Product.Price;
            dbProduct.Quantity = Product.Quantity;
            if (dbProduct == null) return NotFound();
            if (Product.Photo != null)
            {
                if (!_fileService.IsImage(Product.Photo))
                {
                    ModelState.AddModelError("Photo", "The file must be in Image format.");
                    return View(Product);
                }
                int maxSize = 60;
                if (!_fileService.CheckSize(Product.Photo, maxSize))
                {
                    ModelState.AddModelError("Photo", $"The size of the image should not exceed {maxSize} KB");
                    //Product.FilePath = dbProduct.FilePath; submit eleyende shekil silinmesin deye 
                    return View(Product);
                }
                string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets/img");
                _fileService.Delete(path);
                var filename = await _fileService.UploadAsync(Product.Photo);
                dbProduct.FilePath = filename;
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
