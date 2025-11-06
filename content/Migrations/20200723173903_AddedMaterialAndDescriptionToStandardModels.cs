using Microsoft.EntityFrameworkCore.Migrations;

namespace FurnitureBuildingSolution.Migrations
{
    public partial class AddedMaterialAndDescriptionToStandardModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "StandardModels",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Material",
                table: "StandardModels",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "StandardModels");

            migrationBuilder.DropColumn(
                name: "Material",
                table: "StandardModels");
        }
    }
}
