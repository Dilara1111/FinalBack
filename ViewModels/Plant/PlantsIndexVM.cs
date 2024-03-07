using Final_Back.Models;
using System.ComponentModel.DataAnnotations;

namespace Final_Back.ViewModels.Plant
{
    public class PlantsIndexVM
    {
        public List<Products> Products { get; set; }

        #region Pagination
        public int Page { get; set; } = 1;
        public int Take { get; set; } = 9;
        public int Skip { get; set; } 
        public int PageCount { get; set; }
        #endregion

        #region Filter
        public string? Title { get; set; }
        [Display(Name = "MinPrice")]
        public int? MinPrice { get; set; }
        [Display(Name ="MaxPrice")]
        public int? MaxPrice { get; set; }
        #endregion
    }
    
}
