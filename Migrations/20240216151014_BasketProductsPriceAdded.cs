using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Final_Back.Migrations
{
    public partial class BasketProductsPriceAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "BasketProducts",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "BasketProducts");
        }
    }
}
