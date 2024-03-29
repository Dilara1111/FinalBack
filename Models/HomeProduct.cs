﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Final_Back.Models
{
	public class HomeProducts
	{
		public int Id { get; set; }
        [Required(ErrorMessage = "The name must be written"), MinLength(3, ErrorMessage = "Minimum 3 symbols")]

        public string Name { get; set; }
        [Required(ErrorMessage = "The description must be written"), MinLength(5, ErrorMessage = "Minimum 5 symbols")]

        public string Description { get; set; }
        [Required(ErrorMessage = "The price must be written")]

		public double Price { get; set; }
        public string? FilePath { get; set; }
        [NotMapped]
        public IFormFile? Photo { get; set; }
    }
}
