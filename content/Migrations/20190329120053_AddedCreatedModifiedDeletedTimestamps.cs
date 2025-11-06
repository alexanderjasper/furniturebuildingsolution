using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FurnitureBuildingSolution.Migrations
{
    public partial class AddedCreatedModifiedDeletedTimestamps : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "Users",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Deleted",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Modified",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "Roles",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Deleted",
                table: "Roles",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Modified",
                table: "Roles",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "Plates",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Deleted",
                table: "Plates",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Modified",
                table: "Plates",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "Compartments",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Deleted",
                table: "Compartments",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Modified",
                table: "Compartments",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "Bookcases",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Deleted",
                table: "Bookcases",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Modified",
                table: "Bookcases",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Created",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Modified",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Modified",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "Plates");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "Plates");

            migrationBuilder.DropColumn(
                name: "Modified",
                table: "Plates");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "Compartments");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "Compartments");

            migrationBuilder.DropColumn(
                name: "Modified",
                table: "Compartments");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "Bookcases");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "Bookcases");

            migrationBuilder.DropColumn(
                name: "Modified",
                table: "Bookcases");
        }
    }
}
