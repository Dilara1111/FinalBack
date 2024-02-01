using Microsoft.AspNetCore.Mvc;

namespace Final_Back.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
