using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuickFoodDelivery.Persistence.DAL.Migrations
{
    public partial class CourierTablewasUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CourierStatus",
                table: "Courier",
                newName: "Status");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Courier",
                newName: "CourierStatus");
        }
    }
}
