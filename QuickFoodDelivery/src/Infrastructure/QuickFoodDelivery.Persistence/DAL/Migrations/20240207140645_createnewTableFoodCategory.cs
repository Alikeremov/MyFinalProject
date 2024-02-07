using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuickFoodDelivery.Persistence.DAL.Migrations
{
    public partial class createnewTableFoodCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FoodCategoryId",
                table: "Meals",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FoodCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiredAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodCategories", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Meals_FoodCategoryId",
                table: "Meals",
                column: "FoodCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Meals_FoodCategories_FoodCategoryId",
                table: "Meals",
                column: "FoodCategoryId",
                principalTable: "FoodCategories",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Meals_FoodCategories_FoodCategoryId",
                table: "Meals");

            migrationBuilder.DropTable(
                name: "FoodCategories");

            migrationBuilder.DropIndex(
                name: "IX_Meals_FoodCategoryId",
                table: "Meals");

            migrationBuilder.DropColumn(
                name: "FoodCategoryId",
                table: "Meals");
        }
    }
}
