using Microsoft.EntityFrameworkCore.Migrations;

namespace FurnitureBuildingSolution.Migrations
{
    public partial class AddedDoorsDrawersModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasDoor",
                table: "Compartments",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasDrawer",
                table: "Compartments",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Model",
                table: "Compartments",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasDoor",
                table: "Compartments");

            migrationBuilder.DropColumn(
                name: "HasDrawer",
                table: "Compartments");

            migrationBuilder.DropColumn(
                name: "Model",
                table: "Compartments");
        }
    }
}
