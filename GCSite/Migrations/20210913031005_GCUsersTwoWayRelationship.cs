using Microsoft.EntityFrameworkCore.Migrations;

namespace GCSite.Migrations
{
    public partial class GCUsersTwoWayRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OwnedGame_GCUsers_GCUserId",
                table: "OwnedGame");

            migrationBuilder.AlterColumn<int>(
                name: "GCUserId",
                table: "OwnedGame",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OwnedGame_GCUsers_GCUserId",
                table: "OwnedGame",
                column: "GCUserId",
                principalTable: "GCUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OwnedGame_GCUsers_GCUserId",
                table: "OwnedGame");

            migrationBuilder.AlterColumn<int>(
                name: "GCUserId",
                table: "OwnedGame",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_OwnedGame_GCUsers_GCUserId",
                table: "OwnedGame",
                column: "GCUserId",
                principalTable: "GCUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
