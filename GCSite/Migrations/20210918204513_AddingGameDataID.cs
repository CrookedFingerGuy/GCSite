using Microsoft.EntityFrameworkCore.Migrations;

namespace GCSite.Migrations
{
    public partial class AddingGameDataID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OwnedGame_Games_GameDataId",
                table: "OwnedGame");

            migrationBuilder.AlterColumn<int>(
                name: "GameDataId",
                table: "OwnedGame",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OwnedGame_Games_GameDataId",
                table: "OwnedGame",
                column: "GameDataId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OwnedGame_Games_GameDataId",
                table: "OwnedGame");

            migrationBuilder.AlterColumn<int>(
                name: "GameDataId",
                table: "OwnedGame",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_OwnedGame_Games_GameDataId",
                table: "OwnedGame",
                column: "GameDataId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
