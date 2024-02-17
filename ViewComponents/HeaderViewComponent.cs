using Final_Back.DAL;
using Final_Back.Models;
using Final_Back.ViewModels.Header;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Collections.Generic;
using System.Security.Claims;

namespace Final_Back.ViewComponents
{
    public class HeaderViewComponent : ViewComponent
    {
        private readonly AppDbContext _dbContext;
        private readonly UserManager<User> _userManager;
        public HeaderViewComponent(AppDbContext appDbContext, UserManager<User> userManager)
        {
            _dbContext = appDbContext;
            _userManager = userManager;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userManager.GetUserAsync(Request.HttpContext.User);
            if (user != null)
            {
                var basket = await _dbContext.Basket
               .Include(bp => bp.BasketProducts)
               .FirstOrDefaultAsync(b => b.UserId == user.Id);
                List<BasketProduct> quantity = await _dbContext.BasketProducts.Where(x => x.BasketId == basket.Id).ToListAsync();
                int quantityIndex = 0;
                List<BasketProduct> price = await _dbContext.BasketProducts.Where(x => x.BasketId == basket.Id).ToListAsync();
                double priceIndex = 0;
                foreach (var item in quantity)
                {
                    quantityIndex += item.Quantity;
                }
                foreach (var item in price)
                {
                    priceIndex += item.Price;
                }

                HeaderVM headerVMs = new HeaderVM()
                {
                    Quantity = quantityIndex,
                    Price = priceIndex
                };
                return View(headerVMs);
            }
            HeaderVM headerVM = new HeaderVM()
            {
                Quantity = 0,
                Price = 0,
            };
            return View(headerVM);
        }
    }
}
