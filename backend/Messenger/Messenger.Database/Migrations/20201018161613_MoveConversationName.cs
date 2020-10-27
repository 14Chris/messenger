using Microsoft.EntityFrameworkCore.Migrations;

namespace Messenger.Database.Migrations
{
    public partial class MoveConversationName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Conversations");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "UserConversations",
                nullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "UserConversations");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Conversations",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: false,
                defaultValue: "");
        }
    }
}
