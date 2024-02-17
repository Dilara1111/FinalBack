using Final_Back.Areas.Admin.ViewModels.Product;
using Microsoft.AspNetCore.Identity;

namespace Final_Back.Models
{
    public class User : IdentityUser
    {
        public string Fullname { get; set; }
        public Basket Basket { get; set; }

        
    }
}

