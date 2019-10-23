using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace StudyBuddyBackend.Migrations
{
    public partial class UsersInChat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserChat");

            migrationBuilder.CreateTable(
                name: "UsersInChats",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ChatGroupName = table.Column<string>(nullable: true),
                    Username = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersInChats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsersInChats_Chats_ChatGroupName",
                        column: x => x.ChatGroupName,
                        principalTable: "Chats",
                        principalColumn: "GroupName",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UsersInChats_Users_Username",
                        column: x => x.Username,
                        principalTable: "Users",
                        principalColumn: "Username",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UsersInChats_ChatGroupName",
                table: "UsersInChats",
                column: "ChatGroupName");

            migrationBuilder.CreateIndex(
                name: "IX_UsersInChats_Username",
                table: "UsersInChats",
                column: "Username");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UsersInChats");

            migrationBuilder.CreateTable(
                name: "UserChat",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ChatGroupName = table.Column<string>(type: "text", nullable: true),
                    Username = table.Column<string>(type: "character varying", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserChat", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserChat_Chats_ChatGroupName",
                        column: x => x.ChatGroupName,
                        principalTable: "Chats",
                        principalColumn: "GroupName",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserChat_Users_Username",
                        column: x => x.Username,
                        principalTable: "Users",
                        principalColumn: "Username",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserChat_ChatGroupName",
                table: "UserChat",
                column: "ChatGroupName");

            migrationBuilder.CreateIndex(
                name: "IX_UserChat_Username",
                table: "UserChat",
                column: "Username");
        }
    }
}
