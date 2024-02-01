using Final_Back.DAL;
using Final_Back.ViewModels.Home;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Final_Back.Controllers
{
	public class HomeController : Controller
	{
		private readonly AppDbContext _appDbContext;
		public HomeController(AppDbContext appDbContext)
		{

			_appDbContext = appDbContext;
		}
		public async Task<IActionResult> Index()
		{
			HomeIndexVM homeVM = new HomeIndexVM
			{
				ElementorTitle = await _appDbContext.ElementorTitle.FirstOrDefaultAsync(),
                HomeProducts = await _appDbContext.HomeProducts.ToListAsync(),
				OurStory = await _appDbContext.OurStory.FirstOrDefaultAsync(),
                Customers = await _appDbContext.Customers.ToListAsync(),
				GiftCard = await _appDbContext.GiftCard.FirstOrDefaultAsync(),
				Icons = await _appDbContext.Icons.ToListAsync(),
			};
			return View(homeVM);
		}
			
	}
}
