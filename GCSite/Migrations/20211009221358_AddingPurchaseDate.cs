using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GCSite.Migrations
{
    public partial class AddingPurchaseDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ModelPath",
                table: "OwnedGame",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PurchaseDate",
                table: "OwnedGame",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ModelPath",
                table: "OwnedGame");

            migrationBuilder.DropColumn(
                name: "PurchaseDate",
                table: "OwnedGame");
        }
    }
}
