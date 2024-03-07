using Final_Back.DAL;
using Final_Back.Migrations;
using Final_Back.Models;
using Final_Back.ViewModels.Basket;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Final_Back.Controllers
{
    [Authorize]
    public class BasketController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly AppDbContext _dbcontext;
        public BasketController(UserManager<User> userManager, AppDbContext context)
        {
            _userManager = userManager;
            _dbcontext = context;
        }
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();
            var basket = await _dbcontext.Basket
                .Include(bp => bp.BasketProducts)
                .ThenInclude(p => p.Product)
                .FirstOrDefaultAsync(b => b.UserId == user.Id);
            //List<BasketProduct> quantity = await _dbcontext.BasketProducts.Where(x => x.BasketId == basket.Id).ToListAsync();
            //int quantityIndex = 0;
            //foreach (var item in quantity)
            //{
            //    quantityIndex += item.Quantity;
            //}
            var model = new BasketIndexVM();
            if (basket == null) return View(model);
            foreach (var basketProduct in basket.BasketProducts)
            {
                var product = new BasketProductVM
                {
                    Id = basketProduct.Id,
                    Quantity = basketProduct.Quantity,
                    StockQuantity = basketProduct.Product.Quantity,
                    Name = basketProduct.Product.Name,
                    PhotoPath = basketProduct.Product.FilePath,
                    Price = basketProduct.Product.Price,
                };
                model.BasketProducts.Add(product);
            }
            return View(model);
        }

        #region Add
        [HttpPost]
        public async Task<IActionResult> Add(int id , int quantity)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();
            var product = await _dbcontext.Products.FirstOrDefaultAsync(a=>a.Id==id);
            if (product == null) return NotFound();
            var userBasket = await _dbcontext.Basket.FirstOrDefaultAsync(b => b.UserId == user.Id);
            if (userBasket == null)
            {
                userBasket = new Basket
                {
                    UserId = user.Id,
                };
                await _dbcontext.Basket.AddAsync(userBasket);
                await _dbcontext.SaveChangesAsync();
            }
            var basketProduct = await _dbcontext.BasketProducts.
                FirstOrDefaultAsync(bp => bp.BasketId == userBasket.Id && bp.ProductId == id);
            if (basketProduct == null)
            {
                quantity = 1;
                basketProduct = new BasketProduct
                {
                    Price =(int)product.Price,
                    BasketId = userBasket.Id,
                    ProductId = product.Id,
                    Quantity = quantity
                };
                await _dbcontext.BasketProducts.AddAsync(basketProduct);
            }
            else
            {
                basketProduct.Quantity++; 
            }
            await _dbcontext.SaveChangesAsync();

            return StatusCode((int)HttpStatusCode.Created);
        }
        #endregion
        #region Delete
        public async Task<IActionResult> Delete(int Id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();
            var basketProduct = await _dbcontext.BasketProducts
                .FirstOrDefaultAsync(bp => bp.Basket.UserId == user.Id && bp.Id == Id);
            if (basketProduct == null) return NotFound();
            _dbcontext.BasketProducts.Remove(basketProduct);
            await _dbcontext.SaveChangesAsync();
            return Ok();
        }
        #endregion
    }
}
