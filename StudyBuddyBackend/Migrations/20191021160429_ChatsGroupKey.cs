using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace StudyBuddyBackend.Migrations
{
    public partial class ChatsGroupKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Chats_ChatId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Users_Username",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_UserChat_Chats_ChatId",
                table: "UserChat");

            migrationBuilder.DropIndex(
                name: "IX_UserChat_ChatId",
                table: "UserChat");

            migrationBuilder.DropIndex(
                name: "IX_Messages_ChatId",
                table: "Messages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Chats",
                table: "Chats");

            migrationBuilder.DropColumn(
                name: "ChatId",
                table: "UserChat");

            migrationBuilder.DropColumn(
                name: "ChatId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Chats");

            migrationBuilder.AlterColumn<string>(
                name: "Salt",
                table: "Users",
                type: "VARCHAR",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "Users",
                type: "VARCHAR",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "Users",
                type: "VARCHAR",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "Users",
                type: "VARCHAR",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "VARCHAR",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR",
                oldMaxLength: 255);

            migrationBuilder.AddColumn<string>(
                name: "ChatGroupName",
                table: "UserChat",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "Messages",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying");

            migrationBuilder.AddColumn<string>(
                name: "ChatGroupName",
                table: "Messages",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GroupName",
                table: "Chats",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Chats",
                table: "Chats",
                column: "GroupName");

            migrationBuilder.CreateIndex(
                name: "IX_UserChat_ChatGroupName",
                table: "UserChat",
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
                name: "FK_Messages_Users_Username",
                table: "Messages",
                column: "Username",
                principalTable: "Users",
                principalColumn: "Username",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserChat_Chats_ChatGroupName",
                table: "UserChat",
                column: "ChatGroupName",
                principalTable: "Chats",
                principalColumn: "GroupName",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Chats_ChatGroupName",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Users_Username",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_UserChat_Chats_ChatGroupName",
                table: "UserChat");

            migrationBuilder.DropIndex(
                name: "IX_UserChat_ChatGroupName",
                table: "UserChat");

            migrationBuilder.DropIndex(
                name: "IX_Messages_ChatGroupName",
                table: "Messages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Chats",
                table: "Chats");

            migrationBuilder.DropColumn(
                name: "ChatGroupName",
                table: "UserChat");

            migrationBuilder.DropColumn(
                name: "ChatGroupName",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "GroupName",
                table: "Chats");

            migrationBuilder.AlterColumn<string>(
                name: "Salt",
                table: "Users",
                type: "VARCHAR",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VARCHAR",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "Users",
                type: "VARCHAR",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VARCHAR",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "Users",
                type: "VARCHAR",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VARCHAR",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "Users",
                type: "VARCHAR",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VARCHAR",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "VARCHAR",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VARCHAR",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ChatId",
                table: "UserChat",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "Messages",
                type: "character varying",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ChatId",
                table: "Messages",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Chats",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Chats",
                table: "Chats",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_UserChat_ChatId",
                table: "UserChat",
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
                name: "FK_Messages_Users_Username",
                table: "Messages",
                column: "Username",
                principalTable: "Users",
                principalColumn: "Username",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserChat_Chats_ChatId",
                table: "UserChat",
                column: "ChatId",
                principalTable: "Chats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
