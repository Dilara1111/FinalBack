using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Final_Back.Migrations
{
    public partial class check : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "AspNetUsers",
                newName: "Fullname");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Fullname",
                table: "AspNetUsers",
                newName: "Name");
        }
    }
}
