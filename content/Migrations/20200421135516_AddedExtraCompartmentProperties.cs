using Microsoft.EntityFrameworkCore.Migrations;

namespace FurnitureBuildingSolution.Migrations
{
    public partial class AddedExtraCompartmentProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "BackPlatePosition",
                table: "Compartments",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ForceSupport",
                table: "Compartments",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BackPlatePosition",
                table: "Compartments");

            migrationBuilder.DropColumn(
                name: "ForceSupport",
                table: "Compartments");
        }
    }
}
