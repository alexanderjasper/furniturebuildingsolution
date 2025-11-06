using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FurnitureBuildingSolution.Migrations
{
    public partial class AddedMaterial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaterialId",
                table: "Bookcases",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Materials",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    Deleted = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    Thickness = table.Column<double>(nullable: false),
                    SideTextureLocation = table.Column<string>(nullable: false),
                    SurfaceTextureLocation = table.Column<string>(nullable: false),
                    PricePerSquareMeter = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Materials", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bookcases_MaterialId",
                table: "Bookcases",
                column: "MaterialId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookcases_Materials_MaterialId",
                table: "Bookcases",
                column: "MaterialId",
                principalTable: "Materials",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookcases_Materials_MaterialId",
                table: "Bookcases");

            migrationBuilder.DropTable(
                name: "Materials");

            migrationBuilder.DropIndex(
                name: "IX_Bookcases_MaterialId",
                table: "Bookcases");

            migrationBuilder.DropColumn(
                name: "MaterialId",
                table: "Bookcases");
        }
    }
}
