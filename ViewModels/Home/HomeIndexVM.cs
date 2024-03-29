﻿using Final_Back.Models;

namespace Final_Back.ViewModels.Home
{
	public class HomeIndexVM
	{
		public ElementorTitle ElementorTitle { get; set; }
		public List<Products> Products { get; set; }
		public OurStory OurStory { get; set; }
		public List<Customers> Customers { get; set; }
		public GiftCard GiftCard { get; set; }
		public List<Icons> Icons { get; set; }
        public User User { get; set; }
    }
}