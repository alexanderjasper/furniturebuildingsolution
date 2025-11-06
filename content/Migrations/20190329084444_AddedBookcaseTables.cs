using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FurnitureBuildingSolution.Migrations
{
    public partial class AddedBookcaseTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bookcases",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookcases", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RoleName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Plates",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    StartXCoordinate = table.Column<int>(nullable: false),
                    StartYCoordinate = table.Column<int>(nullable: false),
                    EndXCoordinate = table.Column<int>(nullable: false),
                    EndYCoordinate = table.Column<int>(nullable: false),
                    InnerPlate = table.Column<bool>(nullable: false),
                    DbBookcaseId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Plates_Bookcases_DbBookcaseId",
                        column: x => x.DbBookcaseId,
                        principalTable: "Bookcases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Compartments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TopId = table.Column<int>(nullable: true),
                    BottomId = table.Column<int>(nullable: true),
                    LeftId = table.Column<int>(nullable: true),
                    RightId = table.Column<int>(nullable: true),
                    DbBookcaseId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Compartments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Compartments_Plates_BottomId",
                        column: x => x.BottomId,
                        principalTable: "Plates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Compartments_Bookcases_DbBookcaseId",
                        column: x => x.DbBookcaseId,
                        principalTable: "Bookcases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Compartments_Plates_LeftId",
                        column: x => x.LeftId,
                        principalTable: "Plates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Compartments_Plates_RightId",
                        column: x => x.RightId,
                        principalTable: "Plates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Compartments_Plates_TopId",
                        column: x => x.TopId,
                        principalTable: "Plates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Compartments_BottomId",
                table: "Compartments",
                column: "BottomId");

            migrationBuilder.CreateIndex(
                name: "IX_Compartments_DbBookcaseId",
                table: "Compartments",
                column: "DbBookcaseId");

            migrationBuilder.CreateIndex(
                name: "IX_Compartments_LeftId",
                table: "Compartments",
                column: "LeftId");

            migrationBuilder.CreateIndex(
                name: "IX_Compartments_RightId",
                table: "Compartments",
                column: "RightId");

            migrationBuilder.CreateIndex(
                name: "IX_Compartments_TopId",
                table: "Compartments",
                column: "TopId");

            migrationBuilder.CreateIndex(
                name: "IX_Plates_DbBookcaseId",
                table: "Plates",
                column: "DbBookcaseId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Compartments");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Plates");

            migrationBuilder.DropTable(
                name: "Bookcases");
        }
    }
}
