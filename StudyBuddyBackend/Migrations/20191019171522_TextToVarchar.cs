using Microsoft.EntityFrameworkCore.Migrations;

namespace StudyBuddyBackend.Migrations
{
    public partial class TextToVarchar : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                "Salt",
                "Users",
                "VARCHAR",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                "Password",
                "Users",
                "VARCHAR",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                "LastName",
                "Users",
                "VARCHAR",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                "FirstName",
                "Users",
                "VARCHAR",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                "Email",
                "Users",
                "VARCHAR",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                "Username",
                "Users",
                "VARCHAR",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                "Username",
                "UserChat",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                "Username",
                "Messages",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                "Salt",
                "Users",
                "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VARCHAR",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                "Password",
                "Users",
                "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VARCHAR",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                "LastName",
                "Users",
                "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VARCHAR",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                "FirstName",
                "Users",
                "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VARCHAR",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                "Email",
                "Users",
                "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VARCHAR",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                "Username",
                "Users",
                "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VARCHAR",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                "Username",
                "UserChat",
                "text",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                "Username",
                "Messages",
                "text",
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}
