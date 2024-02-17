using Final_Back.DAL;
using Final_Back.Models;
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
        public async Task<IActionResult> Index(PlantsIndexVM model)
        {
            var productFilter = FilterByTitle(model.Title);
            var products = await PaginateProductsAsync(model.Take, model.Page, model.Title);
            model = new PlantsIndexVM 
            {
                //Products = productFilter.ToList(),
                Products = products,
                PageCount = await GetPageCountAsync(model.Take),
                Page = model.Page,
            };
            return View(model);
        }

        public IQueryable<Products> FilterByTitle(string? title)
        {
           return _appDbContext.Products
                .Where(p => !string.IsNullOrEmpty(title) ? p.Name.Contains(title) : true);
        }
        private async Task<int> GetPageCountAsync(int take)
        {
            var products = await _appDbContext.Products.ToListAsync();
            var productsCount = products.Count();
            return (int)Math.Ceiling((decimal)productsCount / take);
        }
           
        private async Task<List<Products>> PaginateProductsAsync(int take, int page, string? title)
        {
            if (title != null)
            {
                var productsWithTitle = await _appDbContext.Products
                 .Where(p => !string.IsNullOrEmpty(title) ? p.Name.Contains(title) : true)
                .OrderBy(p => p.Id)
                .Skip((page - 1) * take)
                .Take(take)
                .ToListAsync();

                return productsWithTitle;
            }
            var products = await _appDbContext.Products
                .OrderBy(p => p.Id)
                .Skip((page - 1) * take)
                .Take(take)
                .ToListAsync();

            return products;
        }
    }
}
