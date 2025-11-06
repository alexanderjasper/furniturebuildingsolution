using Microsoft.EntityFrameworkCore.Migrations;

namespace FurnitureBuildingSolution.Migrations
{
    public partial class AddedUserReferenceToBookcase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Bookcases",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bookcases_UserId",
                table: "Bookcases",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookcases_Users_UserId",
                table: "Bookcases",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookcases_Users_UserId",
                table: "Bookcases");

            migrationBuilder.DropIndex(
                name: "IX_Bookcases_UserId",
                table: "Bookcases");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Bookcases");
        }
    }
}
