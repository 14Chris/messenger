using Microsoft.EntityFrameworkCore.Migrations;

namespace Messenger.Database.Migrations
{
    public partial class StoreGifAndStcikerUrlInDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GifUrl",
                table: "Messages",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StickerUrl",
                table: "Messages",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GifUrl",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "StickerUrl",
                table: "Messages");
        }
    }
}
