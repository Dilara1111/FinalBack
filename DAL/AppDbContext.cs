using Final_Back.Models;
using Microsoft.EntityFrameworkCore;

namespace Final_Back.DAL
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options)
		: base(options) { }
		public DbSet<ElementorTitle> ElementorTitle { get; set; }
		public DbSet<HomeProducts> HomeProducts { get; set; }
		public DbSet<OurStory> OurStory { get; set; }
		public DbSet<Customers> Customers { get; set; }
		public DbSet<GiftCard> GiftCard { get; set; }
		public DbSet<Products> Products { get; set; }
		public DbSet<Icons> Icons { get; set; }
		public DbSet<ImgContainer> ImgContainer { get; set; }
		public DbSet<AboutComponent> AboutComponent { get; set; }
		public DbSet<AboutContainer> AboutContainer { get; set; }

	}
}
