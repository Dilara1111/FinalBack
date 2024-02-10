namespace Final_Back.ViewModels.Basket
{
    public class BasketProductVM
    {
        public int Id { get; set; }
        public string PhotoPath { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public int StockQuantity { get; set; }
        public double Price { get; set; }

    }
}
