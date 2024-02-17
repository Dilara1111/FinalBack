using Final_Back.Areas.Admin.ViewModels.Customer;
using Final_Back.DAL;
using Final_Back.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Final_Back.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin")]
    public class CustomerController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public CustomerController(AppDbContext appDbContext, IWebHostEnvironment webHostEnvironment)
        {
            _dbContext = appDbContext;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            List<Customers> customers = await _dbContext.Customers.ToListAsync();
            
            return View(customers);
        }
        #region Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Customers customer)
        {
            if (!ModelState.IsValid) return View(customer);
            if (!customer.Photo.ContentType.Contains("image/"))
            {
                ModelState.AddModelError("Photo", "The file must be in Image format.");
                return View(customer);
            }
            if (customer.Photo.Length / 1024 > 60)
            {
                ModelState.AddModelError("Photo", "The size of the image should not exceed 60 MB.");
                return View(customer);
            }
            var filename = $"{Guid.NewGuid()}_{customer.Photo.FileName}";
            var path = Path.Combine(_webHostEnvironment.WebRootPath, "assets/img", filename);

            using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
            {
                await customer.Photo.CopyToAsync(fileStream);
            }
            customer.FilePath = filename;
            await _dbContext.Customers.AddAsync(customer); 
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        #endregion
        #region Update
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var Customer = await _dbContext.Customers.FindAsync(id);
            if (id == null)
            {
                return BadRequest();
            }
            var model = new CustomerUpdateVM
            {
                Id = Customer.Id,
                Name = Customer.Name,
                Description = Customer.Description,
                FilePath = Customer.FilePath,
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Update(CustomerUpdateVM Customer, int id)
        {

            if (id == Customer.Id) return BadRequest();

            if (!ModelState.IsValid) return View(Customer);
            var dbCustomer = await _dbContext.Customers.FindAsync(id);
            dbCustomer.Name = Customer.Name;
            dbCustomer.Description = Customer.Description;
            dbCustomer.FilePath = Customer.FilePath;
            if (dbCustomer == null) return NotFound();
            if(Customer.Photo != null)
            {
                if (!Customer.Photo.ContentType.Contains("image/"))
                {
                    ModelState.AddModelError("Photo", "The file must be in Image format.");
                    return View(Customer);
                }
                if (Customer.Photo.Length / 1024 > 60)
                {
                    ModelState.AddModelError("Photo", "The size of the image should not exceed 60 MB.");
                    return View(Customer);
                }

                if (Customer.Photo != null)
                {
                    string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets/img", dbCustomer.FilePath);
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                    _dbContext.Customers.Remove(dbCustomer);
                }
            }           
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        #endregion
        #region Delete
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var dbCustomer = await _dbContext.Customers.FindAsync(id);
            if (dbCustomer == null) return NotFound();
            var path = Path.Combine(_webHostEnvironment.WebRootPath, "assets/img");
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            _dbContext.Customers.Remove(dbCustomer);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        #endregion
    }
}
