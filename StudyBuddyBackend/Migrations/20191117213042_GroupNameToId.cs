using Microsoft.EntityFrameworkCore.Migrations;

namespace StudyBuddyBackend.Migrations
{
    public partial class GroupNameToId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Chats_ChatGroupName",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersInChats_Chats_ChatGroupName",
                table: "UsersInChats");

            migrationBuilder.DropIndex(
                name: "IX_UsersInChats_ChatGroupName",
                table: "UsersInChats");

            migrationBuilder.DropIndex(
                name: "IX_Messages_ChatGroupName",
                table: "Messages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Chats",
                table: "Chats");

            migrationBuilder.DropColumn(
                name: "ChatGroupName",
                table: "UsersInChats");

            migrationBuilder.DropColumn(
                name: "ChatGroupName",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "GroupName",
                table: "Chats");

            migrationBuilder.AddColumn<string>(
                name: "ChatId",
                table: "UsersInChats",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ChatId",
                table: "Messages",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "Chats",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Chats",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Chats",
                table: "Chats",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_UsersInChats_ChatId",
                table: "UsersInChats",
                column: "ChatId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ChatId",
                table: "Messages",
                column: "ChatId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Chats_ChatId",
                table: "Messages",
                column: "ChatId",
                principalTable: "Chats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersInChats_Chats_ChatId",
                table: "UsersInChats",
                column: "ChatId",
                principalTable: "Chats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Chats_ChatId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersInChats_Chats_ChatId",
                table: "UsersInChats");

            migrationBuilder.DropIndex(
                name: "IX_UsersInChats_ChatId",
                table: "UsersInChats");

            migrationBuilder.DropIndex(
                name: "IX_Messages_ChatId",
                table: "Messages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Chats",
                table: "Chats");

            migrationBuilder.DropColumn(
                name: "ChatId",
                table: "UsersInChats");

            migrationBuilder.DropColumn(
                name: "ChatId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Chats");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Chats");

            migrationBuilder.AddColumn<string>(
                name: "ChatGroupName",
                table: "UsersInChats",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ChatGroupName",
                table: "Messages",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GroupName",
                table: "Chats",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Chats",
                table: "Chats",
                column: "GroupName");

            migrationBuilder.CreateIndex(
                name: "IX_UsersInChats_ChatGroupName",
                table: "UsersInChats",
                column: "ChatGroupName");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ChatGroupName",
                table: "Messages",
                column: "ChatGroupName");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Chats_ChatGroupName",
                table: "Messages",
                column: "ChatGroupName",
                principalTable: "Chats",
                principalColumn: "GroupName",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersInChats_Chats_ChatGroupName",
                table: "UsersInChats",
                column: "ChatGroupName",
                principalTable: "Chats",
                principalColumn: "GroupName",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
