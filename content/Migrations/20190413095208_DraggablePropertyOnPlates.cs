using Microsoft.EntityFrameworkCore.Migrations;

namespace FurnitureBuildingSolution.Migrations
{
    public partial class DraggablePropertyOnPlates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Draggable",
                table: "Plates",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Draggable",
                table: "Plates");
        }
    }
}
