using Microsoft.EntityFrameworkCore.Migrations;

namespace FurnitureBuildingSolution.Migrations
{
    public partial class AddedInnerPlateStartAndEnd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "InnerPlate",
                table: "Plates",
                newName: "InnerPlateStart");

            migrationBuilder.AddColumn<bool>(
                name: "InnerPlateEnd",
                table: "Plates",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InnerPlateEnd",
                table: "Plates");

            migrationBuilder.RenameColumn(
                name: "InnerPlateStart",
                table: "Plates",
                newName: "InnerPlate");
        }
    }
}
