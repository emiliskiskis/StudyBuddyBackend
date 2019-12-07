using Microsoft.EntityFrameworkCore.Migrations;

namespace StudyBuddyBackend.Migrations
{
    public partial class UserRequired : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
        }
    }
}
