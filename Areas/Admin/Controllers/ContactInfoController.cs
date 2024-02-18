using Final_Back.DAL;
using Final_Back.Helpers;
using Final_Back.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Final_Back.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles="SuperAdmin")]
    public class ContactInfoController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly IFileService _fileService;

        public ContactInfoController(AppDbContext appDbContext, IFileService fileService)
        {
            _appDbContext = appDbContext;
            _fileService = fileService;
        }

        public async Task<IActionResult> Index()
        {
            ContactInfo contactInfo = await _appDbContext.ContactInfo.FirstOrDefaultAsync();
            return View(contactInfo);
        }
        #region Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ContactInfo contactInfo)
        {
            if(!ModelState.IsValid) return View(contactInfo);
            await _appDbContext.ContactInfo.AddAsync(contactInfo);
            await _appDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        #endregion
        #region Update
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var contact = await _appDbContext.ContactInfo.FirstOrDefaultAsync(c=> c.Id ==id );
            if (contact == null) return NotFound();
            var model = new ContactInfo
            {
                Id = contact.Id,
                PhoneNumber = contact.PhoneNumber,
                Email = contact.Email,
                Title = contact.Title,
                Address = contact.Address,
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Update(ContactInfo contactInfo, int id)
        {
            if (id != contactInfo.Id) return BadRequest();
            if (!ModelState.IsValid) return View(contactInfo);
            var dbcontactInfo = await _appDbContext.ContactInfo.FindAsync(id);
            dbcontactInfo.Email = contactInfo.Email;
            //dbcontactInfo.Title = contactInfo.Title;
            dbcontactInfo.PhoneNumber = contactInfo.PhoneNumber;
            dbcontactInfo.Address = contactInfo.Address;
            if (dbcontactInfo == null) return NotFound();
            await _appDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        #endregion
        #region Delete
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var dbContactInfo = await _appDbContext.ContactInfo.FindAsync(id);
            if (dbContactInfo == null) return NotFound();
            _appDbContext.ContactInfo.Remove(dbContactInfo);
            await _appDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        #endregion
    }
}




