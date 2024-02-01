using Final_Back.DAL;
using Final_Back.ViewModels.Plant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Final_Back.Controllers
{
    public class PlantController : Controller
    {
        private readonly AppDbContext _appDbContext;
        public PlantController(AppDbContext appDbContext)
        {

            _appDbContext = appDbContext;
        }
        public async Task<IActionResult> Index()
        {
            PlantsIndexVM plantsIndexVM = new PlantsIndexVM
            {
                Products = await _appDbContext.Products.ToListAsync(),
            };
            return View(plantsIndexVM);
        }
    }
}
