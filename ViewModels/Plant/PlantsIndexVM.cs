using Final_Back.Models;
namespace Final_Back.ViewModels.Plant
{
    public class PlantsIndexVM
    {
        public List<Products> Products { get; set; }
        public int Page { get; set; } = 1;
        public int Take { get; set; } = 9;
        public int Skip { get; set; } 
        public int PageCount { get; set; } 
        public string? Title { get; set; }
    }
    
}
