﻿namespace Final_Back.Models
{
    public class BasketProduct
    {
        public int Id { get; set; }
        public int BasketId { get; set; }
        public Basket Basket { get; set; }
        public int Quantity { get; set; }
        public int ProductId { get; set; }
        public Products Product { get; set; }
        public double Price { get; set; }

    }
}
