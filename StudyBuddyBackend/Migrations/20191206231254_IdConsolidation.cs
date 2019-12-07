using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace StudyBuddyBackend.Migrations
{
    public partial class IdConsolidation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Feedback_Users_AuthorUsername",
                table: "Feedback");

            migrationBuilder.DropForeignKey(
                name: "FK_Feedback_Users_ReviewerUsername",
                table: "Feedback");

            migrationBuilder.DropForeignKey(
                name: "FK_ProfilePictures_Users_UserUsername",
                table: "ProfilePictures");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersInChats_Chats_ChatId",
                table: "UsersInChats");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersInChats_Users_Username",
                table: "UsersInChats");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UsersInChats",
                table: "UsersInChats");

            migrationBuilder.DropIndex(
                name: "IX_UsersInChats_ChatId",
                table: "UsersInChats");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProfilePictures",
                table: "ProfilePictures");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Feedback",
                table: "Feedback");

            migrationBuilder.DropIndex(
                name: "IX_Feedback_AuthorUsername",
                table: "Feedback");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "UsersInChats");

            migrationBuilder.DropColumn(
                name: "UserUsername",
                table: "ProfilePictures");

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "UsersInChats",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ChatId",
                table: "UsersInChats",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "ProfilePictures",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "ReviewerUsername",
                table: "Feedback",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AuthorUsername",
                table: "Feedback",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UsersInChats",
                table: "UsersInChats",
                columns: new[] { "ChatId", "Username" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProfilePictures",
                table: "ProfilePictures",
                column: "Username");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Feedback",
                table: "Feedback",
                columns: new[] { "AuthorUsername", "ReviewerUsername" });

            migrationBuilder.AddForeignKey(
                name: "FK_Feedback_Users_AuthorUsername",
                table: "Feedback",
                column: "AuthorUsername",
                principalTable: "Users",
                principalColumn: "Username",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Feedback_Users_ReviewerUsername",
                table: "Feedback",
                column: "ReviewerUsername",
                principalTable: "Users",
                principalColumn: "Username",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProfilePictures_Users_Username",
                table: "ProfilePictures",
                column: "Username",
                principalTable: "Users",
                principalColumn: "Username",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersInChats_Chats_ChatId",
                table: "UsersInChats",
                column: "ChatId",
                principalTable: "Chats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersInChats_Users_Username",
                table: "UsersInChats",
                column: "Username",
                principalTable: "Users",
                principalColumn: "Username",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Feedback_Users_AuthorUsername",
                table: "Feedback");

            migrationBuilder.DropForeignKey(
                name: "FK_Feedback_Users_ReviewerUsername",
                table: "Feedback");

            migrationBuilder.DropForeignKey(
                name: "FK_ProfilePictures_Users_Username",
                table: "ProfilePictures");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersInChats_Chats_ChatId",
                table: "UsersInChats");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersInChats_Users_Username",
                table: "UsersInChats");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UsersInChats",
                table: "UsersInChats");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProfilePictures",
                table: "ProfilePictures");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Feedback",
                table: "Feedback");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "ProfilePictures");

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "UsersInChats",
                type: "character varying",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "ChatId",
                table: "UsersInChats",
                type: "text",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "UsersInChats",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<string>(
                name: "UserUsername",
                table: "ProfilePictures",
                type: "character varying",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "ReviewerUsername",
                table: "Feedback",
                type: "character varying",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "AuthorUsername",
                table: "Feedback",
                type: "character varying",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddPrimaryKey(
                name: "PK_UsersInChats",
                table: "UsersInChats",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProfilePictures",
                table: "ProfilePictures",
                column: "UserUsername");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Feedback",
                table: "Feedback",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_UsersInChats_ChatId",
                table: "UsersInChats",
                column: "ChatId");

            migrationBuilder.CreateIndex(
                name: "IX_Feedback_AuthorUsername",
                table: "Feedback",
                column: "AuthorUsername");

            migrationBuilder.AddForeignKey(
                name: "FK_Feedback_Users_AuthorUsername",
                table: "Feedback",
                column: "AuthorUsername",
                principalTable: "Users",
                principalColumn: "Username",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Feedback_Users_ReviewerUsername",
                table: "Feedback",
                column: "ReviewerUsername",
                principalTable: "Users",
                principalColumn: "Username",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProfilePictures_Users_UserUsername",
                table: "ProfilePictures",
                column: "UserUsername",
                principalTable: "Users",
                principalColumn: "Username",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersInChats_Chats_ChatId",
                table: "UsersInChats",
                column: "ChatId",
                principalTable: "Chats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersInChats_Users_Username",
                table: "UsersInChats",
                column: "Username",
                principalTable: "Users",
                principalColumn: "Username",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
