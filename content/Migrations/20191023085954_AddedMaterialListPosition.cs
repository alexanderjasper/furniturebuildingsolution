using Microsoft.EntityFrameworkCore.Migrations;

namespace FurnitureBuildingSolution.Migrations
{
    public partial class AddedMaterialListPosition : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ListPosition",
                table: "Materials",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ListPosition",
                table: "Materials");
        }
    }
}
