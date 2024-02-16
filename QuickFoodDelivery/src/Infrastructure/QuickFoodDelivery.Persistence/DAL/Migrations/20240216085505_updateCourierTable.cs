using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuickFoodDelivery.Persistence.DAL.Migrations
{
    public partial class updateCourierTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Courier",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Courier");
        }
    }
}
