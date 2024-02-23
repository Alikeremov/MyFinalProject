using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuickFoodDelivery.Persistence.DAL.Migrations
{
    public partial class updateReviewsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Courtesy",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "FoodQuality",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "PriceQuality",
                table: "Reviews");

            migrationBuilder.RenameColumn(
                name: "Punctuality",
                table: "Reviews",
                newName: "Quality");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Quality",
                table: "Reviews",
                newName: "Punctuality");

            migrationBuilder.AddColumn<int>(
                name: "Courtesy",
                table: "Reviews",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FoodQuality",
                table: "Reviews",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PriceQuality",
                table: "Reviews",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
