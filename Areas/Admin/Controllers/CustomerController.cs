using Final_Back.Areas.Admin.ViewModels.Customer;
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
    public class CustomerController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IFileService _fileService;

        public CustomerController(AppDbContext appDbContext,
            IWebHostEnvironment webHostEnvironment,
            IFileService fileService)
        {
            _dbContext = appDbContext;
            _webHostEnvironment = webHostEnvironment;
            _fileService = fileService;
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
            if (!_fileService.IsImage(customer.Photo))
            {
                ModelState.AddModelError("Photo", "The file must be in Image format.");
                return View(customer);
            }
            int maxSize = 100;
            if (!_fileService.CheckSize(customer.Photo, maxSize))
            {
                ModelState.AddModelError("Photo", $"The size of the image should not exceed {maxSize} KB.");
                return View(customer);
            }
            var filename = await _fileService.UploadAsync(customer.Photo);
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
            var Customer = await _dbContext.Customers.FirstOrDefaultAsync(c => c.Id == id);
           
            if (Customer == null) return NotFound();            
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

            if (id != Customer.Id) return BadRequest();

            if (!ModelState.IsValid) return View(Customer);
            var dbCustomer = await _dbContext.Customers.FindAsync(id);
            dbCustomer.Name = Customer.Name;
            dbCustomer.Description = Customer.Description;
            if (Customer.Photo != null) 
            {
                if(_fileService.IsImage(Customer.Photo))
                {
                    ModelState.AddModelError("Photo", "The file must be in Image format.");
                    return View(Customer);
                }
                int maxSize = 60;
                if (_fileService.CheckSize(Customer.Photo, maxSize))
                {
                    ModelState.AddModelError("Photo", $"The size of the image should not exceed {maxSize} MB");
                    //Customer.FilePath = dbCustomer.FilePath; submit eleyende shekil silinmesin deye 
                    return View(Customer);
                }
                string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets/img");
                _fileService.Delete(path);
                var filename = await _fileService.UploadAsync(Customer.Photo);
                dbCustomer.FilePath = filename;
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
            _fileService.Delete(path);
            _dbContext.Customers.Remove(dbCustomer);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        #endregion
    }
}
