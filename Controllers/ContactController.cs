using Final_Back.DAL;
using Final_Back.Models;
using Final_Back.ViewModels;
using Final_Back.ViewModels.Contact;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

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
        [HttpPost]
        public async Task<IActionResult> Add(CreateMessage model)
        {
            if (!ModelState.IsValid) return View(model);
            Message message = new Message()
            {
                MessageInfo = model.MessageInfo,
                Email = model.Email,
                Name = model.Name,
            };
            await _appDbContext.Message.AddAsync(message);
            await _appDbContext.SaveChangesAsync();
            return StatusCode((int)HttpStatusCode.Created);
        }
    }
}
