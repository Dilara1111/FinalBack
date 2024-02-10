using Final_Back.DAL;
using Final_Back.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
var connectionString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<AppDbContext>(
	x => x.UseSqlServer(connectionString)
	);

builder.Services.AddIdentity<User, IdentityRole>(
	options =>
	{
		options.Password.RequireNonAlphanumeric = false;
		options.Password.RequireDigit = true;
		options.Password.RequireLowercase = true;
		options.Password.RequireUppercase = true;
		options.Password.RequiredLength = 8;
		options.User.RequireUniqueEmail = true;
		//options.SignIn.RequireConfirmedEmail=true; emaili tesdiqlememish daxil ola bilmesin
	}
	).AddEntityFrameworkStores<AppDbContext>();
var app = builder.Build();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
	name: "areas",
	pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{Id?}");


app.MapDefaultControllerRoute();
app.Run();
