using Final_Back.DAL;
using Final_Back.ViewModels.About;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Final_Back.Controllers
{
    public class AboutController : Controller
    {
        private readonly AppDbContext _appDbContext;
        public AboutController(AppDbContext appDbContext)
        {

            _appDbContext = appDbContext;
        }
        public async Task<IActionResult> Index()
        {
            AboutIndexVM aboutVM = new AboutIndexVM
            {
                ImgContainer = await _appDbContext.ImgContainer.ToListAsync(),
                AboutComponent = await _appDbContext.AboutComponent.FirstOrDefaultAsync(),
                AboutContainer = await _appDbContext.AboutContainer.FirstOrDefaultAsync(),                
            };

            return View(aboutVM);
        }
    }
}
