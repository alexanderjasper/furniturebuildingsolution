using Microsoft.EntityFrameworkCore.Migrations;

namespace FurnitureBuildingSolution.Migrations
{
    public partial class AddedBookcaseName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Bookcases",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Bookcases");
        }
    }
}
