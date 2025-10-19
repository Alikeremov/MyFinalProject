using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuickFoodDelivery.Persistence.DAL.Migrations
{
    public partial class updateRestaurantsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Popularity",
                table: "Restaurants",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Popularity",
                table: "Restaurants");
        }
    }
}
