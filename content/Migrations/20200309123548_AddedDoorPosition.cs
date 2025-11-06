using Microsoft.EntityFrameworkCore.Migrations;

namespace FurnitureBuildingSolution.Migrations
{
    public partial class AddedDoorPosition : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DoorPosition",
                table: "Compartments",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DoorPosition",
                table: "Compartments");
        }
    }
}
