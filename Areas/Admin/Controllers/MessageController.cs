using Final_Back.DAL;
using Final_Back.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Final_Back.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin")]
    public class MessageController : Controller
    {
        private readonly AppDbContext _appDbContext;
        public MessageController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<IActionResult> Index()
        {
            var  messages = await _appDbContext.Message.ToListAsync();
            return View(messages);
        }
        #region Delete
        public async Task<IActionResult> Delete(int id)
        {
            var dbMessage = await _appDbContext.Message.FindAsync(id);
            if (dbMessage == null) return NotFound();
            _appDbContext.Message.Remove(dbMessage);
            await _appDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        #endregion

    }
}
