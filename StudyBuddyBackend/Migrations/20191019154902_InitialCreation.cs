using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace StudyBuddyBackend.Migrations
{
    public partial class InitialCreation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "Chats",
                table => new
                {
                    Id = table.Column<int>()
                        .Annotation("Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chats", x => x.Id);
                });

            migrationBuilder.CreateTable(
                "Users",
                table => new
                {
                    Username = table.Column<string>(),
                    Password = table.Column<string>(),
                    Salt = table.Column<string>(),
                    FirstName = table.Column<string>(),
                    LastName = table.Column<string>(),
                    Email = table.Column<string>()
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Username);
                });

            migrationBuilder.CreateTable(
                "Messages",
                table => new
                {
                    Id = table.Column<int>()
                        .Annotation("Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Username = table.Column<string>(),
                    Text = table.Column<string>(nullable: true),
                    SentAt = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    ChatId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        "FK_Messages_Chats_ChatId",
                        x => x.ChatId,
                        "Chats",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_Messages_Users_Username",
                        x => x.Username,
                        "Users",
                        "Username",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "UserChat",
                table => new
                {
                    Id = table.Column<int>()
                        .Annotation("Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ChatId = table.Column<int>(nullable: true),
                    Username = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserChat", x => x.Id);
                    table.ForeignKey(
                        "FK_UserChat_Chats_ChatId",
                        x => x.ChatId,
                        "Chats",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_UserChat_Users_Username",
                        x => x.Username,
                        "Users",
                        "Username",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                "IX_Messages_ChatId",
                "Messages",
                "ChatId");

            migrationBuilder.CreateIndex(
                "IX_Messages_Username",
                "Messages",
                "Username");

            migrationBuilder.CreateIndex(
                "IX_UserChat_ChatId",
                "UserChat",
                "ChatId");

            migrationBuilder.CreateIndex(
                "IX_UserChat_Username",
                "UserChat",
                "Username");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "Messages");

            migrationBuilder.DropTable(
                "UserChat");

            migrationBuilder.DropTable(
                "Chats");

            migrationBuilder.DropTable(
                "Users");
        }
    }
}
