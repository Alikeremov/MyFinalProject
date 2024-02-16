using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuickFoodDelivery.Persistence.DAL.Migrations
{
    public partial class updateEmploymentTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ActionName",
                table: "Employments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ControllerName",
                table: "Employments",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActionName",
                table: "Employments");

            migrationBuilder.DropColumn(
                name: "ControllerName",
                table: "Employments");
        }
    }
}
