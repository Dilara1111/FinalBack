using Final_Back.DAL;
using Final_Back.ViewModels.Contact;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Final_Back.Controllers
{
    public class ContactController : Controller
    {
        private readonly AppDbContext _appDbContext;
        public ContactController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<IActionResult> Index()
        {
            ContactIndexVM contactVM = new ContactIndexVM
            {
                ContactInfo = await _appDbContext.ContactInfo.FirstOrDefaultAsync()
            };
            return View(contactVM);
        }
    }
}
