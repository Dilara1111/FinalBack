using Final_Back.DAL;
using Final_Back.Models;
using Final_Back.ViewModels.Home;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Final_Back.Controllers
{
	public class HomeController : Controller
	{
		private readonly AppDbContext _appDbContext;
		private readonly UserManager<User> _user;
		private readonly SignInManager<User> _signInManager;
		public HomeController(AppDbContext appDbContext, UserManager<User> user, SignInManager<User> signInManager)
		{

			_appDbContext = appDbContext;
			_user = user;
			_signInManager = signInManager;
		}
		public async Task<IActionResult> Index()
		{
			//if (_signInManager.IsSignedIn(HttpContext.User))
			//{
			//	User currentUser = await _user.GetUserAsync(HttpContext.User);
			//}
			HomeIndexVM homeVM = new HomeIndexVM
			{
				ElementorTitle = await _appDbContext.ElementorTitle.FirstOrDefaultAsync(),
                Products = await _appDbContext.Products.Skip(6).Take(6).ToListAsync(),
				OurStory = await _appDbContext.OurStory.FirstOrDefaultAsync(),
                Customers = await _appDbContext.Customers.ToListAsync(),
				GiftCard = await _appDbContext.GiftCard.FirstOrDefaultAsync(),
				Icons = await _appDbContext.Icons.ToListAsync(),
				User = await _user.GetUserAsync(HttpContext.User),
			};
			return View(homeVM);
		}
			
	}
}
