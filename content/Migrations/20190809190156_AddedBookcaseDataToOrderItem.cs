using Microsoft.EntityFrameworkCore.Migrations;

namespace FurnitureBuildingSolution.Migrations
{
    public partial class AddedBookcaseDataToOrderItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "BookcaseId",
                table: "OrderItems",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_BookcaseId",
                table: "OrderItems",
                column: "BookcaseId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Bookcases_BookcaseId",
                table: "OrderItems",
                column: "BookcaseId",
                principalTable: "Bookcases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Bookcases_BookcaseId",
                table: "OrderItems");

            migrationBuilder.DropIndex(
                name: "IX_OrderItems_BookcaseId",
                table: "OrderItems");

            migrationBuilder.AlterColumn<int>(
                name: "BookcaseId",
                table: "OrderItems",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
