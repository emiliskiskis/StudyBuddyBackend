using Microsoft.EntityFrameworkCore.Migrations;

namespace StudyBuddyBackend.Migrations
{
    public partial class FeedbackFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Feedback_Users_ReviewerUsername",
                table: "Feedback");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Feedback",
                table: "Feedback");

            migrationBuilder.DropIndex(
                name: "IX_Feedback_ReviewerUsername",
                table: "Feedback");

            migrationBuilder.DropColumn(
                name: "ReviewerUsername",
                table: "Feedback");

            migrationBuilder.AlterColumn<string>(
                name: "Comment",
                table: "Feedback",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RevieweeUsername",
                table: "Feedback",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Feedback",
                table: "Feedback",
                columns: new[] { "AuthorUsername", "RevieweeUsername" });

            migrationBuilder.CreateIndex(
                name: "IX_Feedback_RevieweeUsername",
                table: "Feedback",
                column: "RevieweeUsername");

            migrationBuilder.AddForeignKey(
                name: "FK_Feedback_Users_RevieweeUsername",
                table: "Feedback",
                column: "RevieweeUsername",
                principalTable: "Users",
                principalColumn: "Username",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Feedback_Users_RevieweeUsername",
                table: "Feedback");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Feedback",
                table: "Feedback");

            migrationBuilder.DropIndex(
                name: "IX_Feedback_RevieweeUsername",
                table: "Feedback");

            migrationBuilder.DropColumn(
                name: "RevieweeUsername",
                table: "Feedback");

            migrationBuilder.AlterColumn<string>(
                name: "Comment",
                table: "Feedback",
                type: "text",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<string>(
                name: "ReviewerUsername",
                table: "Feedback",
                type: "character varying",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Feedback",
                table: "Feedback",
                columns: new[] { "AuthorUsername", "ReviewerUsername" });

            migrationBuilder.CreateIndex(
                name: "IX_Feedback_ReviewerUsername",
                table: "Feedback",
                column: "ReviewerUsername");

            migrationBuilder.AddForeignKey(
                name: "FK_Feedback_Users_ReviewerUsername",
                table: "Feedback",
                column: "ReviewerUsername",
                principalTable: "Users",
                principalColumn: "Username",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
