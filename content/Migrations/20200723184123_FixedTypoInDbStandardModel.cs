using Microsoft.EntityFrameworkCore.Migrations;

namespace FurnitureBuildingSolution.Migrations
{
    public partial class FixedTypoInDbStandardModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StandardModels_Bookcases_BookaseId",
                table: "StandardModels");

            migrationBuilder.RenameColumn(
                name: "BookaseId",
                table: "StandardModels",
                newName: "BookcaseId");

            migrationBuilder.RenameIndex(
                name: "IX_StandardModels_BookaseId",
                table: "StandardModels",
                newName: "IX_StandardModels_BookcaseId");

            migrationBuilder.AddForeignKey(
                name: "FK_StandardModels_Bookcases_BookcaseId",
                table: "StandardModels",
                column: "BookcaseId",
                principalTable: "Bookcases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StandardModels_Bookcases_BookcaseId",
                table: "StandardModels");

            migrationBuilder.RenameColumn(
                name: "BookcaseId",
                table: "StandardModels",
                newName: "BookaseId");

            migrationBuilder.RenameIndex(
                name: "IX_StandardModels_BookcaseId",
                table: "StandardModels",
                newName: "IX_StandardModels_BookaseId");

            migrationBuilder.AddForeignKey(
                name: "FK_StandardModels_Bookcases_BookaseId",
                table: "StandardModels",
                column: "BookaseId",
                principalTable: "Bookcases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
