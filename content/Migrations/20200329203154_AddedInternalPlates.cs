using Microsoft.EntityFrameworkCore.Migrations;

namespace FurnitureBuildingSolution.Migrations
{
    public partial class AddedInternalPlates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ParentCompartmentId",
                table: "Plates",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Plates_ParentCompartmentId",
                table: "Plates",
                column: "ParentCompartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Plates_Compartments_ParentCompartmentId",
                table: "Plates",
                column: "ParentCompartmentId",
                principalTable: "Compartments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Plates_Compartments_ParentCompartmentId",
                table: "Plates");

            migrationBuilder.DropIndex(
                name: "IX_Plates_ParentCompartmentId",
                table: "Plates");

            migrationBuilder.DropColumn(
                name: "ParentCompartmentId",
                table: "Plates");
        }
    }
}
