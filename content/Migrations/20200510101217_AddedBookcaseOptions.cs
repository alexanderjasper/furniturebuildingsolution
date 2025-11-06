using Microsoft.EntityFrameworkCore.Migrations;

namespace FurnitureBuildingSolution.Migrations
{
    public partial class AddedBookcaseOptions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "BaseHeight",
                table: "Bookcases",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsWallSuspended",
                table: "Bookcases",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "PriceIndex",
                table: "Bookcases",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "SkirtingBoardDepth",
                table: "Bookcases",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "SkirtingBoardHeight",
                table: "Bookcases",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "StartingPrice",
                table: "Bookcases",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BaseHeight",
                table: "Bookcases");

            migrationBuilder.DropColumn(
                name: "IsWallSuspended",
                table: "Bookcases");

            migrationBuilder.DropColumn(
                name: "PriceIndex",
                table: "Bookcases");

            migrationBuilder.DropColumn(
                name: "SkirtingBoardDepth",
                table: "Bookcases");

            migrationBuilder.DropColumn(
                name: "SkirtingBoardHeight",
                table: "Bookcases");

            migrationBuilder.DropColumn(
                name: "StartingPrice",
                table: "Bookcases");
        }
    }
}
