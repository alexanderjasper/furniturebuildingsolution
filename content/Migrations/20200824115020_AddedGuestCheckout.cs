using Microsoft.EntityFrameworkCore.Migrations;

namespace FurnitureBuildingSolution.Migrations
{
    public partial class AddedGuestCheckout : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmailAddress",
                table: "Orders",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailAddress",
                table: "Orders");
        }
    }
}
