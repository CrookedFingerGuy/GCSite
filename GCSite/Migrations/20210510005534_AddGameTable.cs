using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GCSite.Migrations
{
    public partial class AddGameTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GameName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Developer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Publisher = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Platform = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Genre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReleaseDateUs = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReleaseDateJp = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReleaseDateEu = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ThumbnailPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CoverArtPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ManualScanPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ThreeDBoxmodelPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RetailPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CurrentValueCib = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CurrentValueMo = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    GameSizeMb = table.Column<long>(type: "bigint", nullable: true),
                    GamePlayLength = table.Column<long>(type: "bigint", nullable: true),
                    MinSpecs = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BoxStyle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BoxSize = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MediaType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MediaCount = table.Column<int>(type: "int", nullable: true),
                    MediaScanPath = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Games");
        }
    }
}
