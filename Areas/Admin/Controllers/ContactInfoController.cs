using Final_Back.DAL;
using Final_Back.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Final_Back.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ContactInfoController : Controller
    {
        private readonly AppDbContext _appDbContext;
        public ContactInfoController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<IActionResult> Index()
        {
            ContactInfo contactInfo = await _appDbContext.ContactInfo.FirstOrDefaultAsync();
            return View(contactInfo);
        }
        #region Create
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
            if (id == null) return BadRequest();
            ContactInfo contactInfo = await _appDbContext.ContactInfo.FindAsync(id);
            if (contactInfo == null) return NotFound();
            return View(contactInfo);
        }
        [HttpPost]
        public async Task<IActionResult> Update(ContactInfo contactInfo, int id)
        {
            if (id == null) return BadRequest();
            contactInfo = await _appDbContext.ContactInfo.FindAsync(id);
            if (contactInfo == null) return NotFound();
            if (!ModelState.IsValid) return View(contactInfo);
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




