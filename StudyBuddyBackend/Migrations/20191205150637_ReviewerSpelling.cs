using Microsoft.EntityFrameworkCore.Migrations;

namespace StudyBuddyBackend.Migrations
{
    public partial class ReviewerSpelling : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Feedbacks_Users_RevieweeUsername",
                table: "Feedbacks");

            migrationBuilder.DropIndex(
                name: "IX_Feedbacks_RevieweeUsername",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "RevieweeUsername",
                table: "Feedbacks");

            migrationBuilder.AddColumn<string>(
                name: "ReviewerUsername",
                table: "Feedbacks",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_ReviewerUsername",
                table: "Feedbacks",
                column: "ReviewerUsername");

            migrationBuilder.AddForeignKey(
                name: "FK_Feedbacks_Users_ReviewerUsername",
                table: "Feedbacks",
                column: "ReviewerUsername",
                principalTable: "Users",
                principalColumn: "Username",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Feedbacks_Users_ReviewerUsername",
                table: "Feedbacks");

            migrationBuilder.DropIndex(
                name: "IX_Feedbacks_ReviewerUsername",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "ReviewerUsername",
                table: "Feedbacks");

            migrationBuilder.AddColumn<string>(
                name: "RevieweeUsername",
                table: "Feedbacks",
                type: "character varying",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_RevieweeUsername",
                table: "Feedbacks",
                column: "RevieweeUsername");

            migrationBuilder.AddForeignKey(
                name: "FK_Feedbacks_Users_RevieweeUsername",
                table: "Feedbacks",
                column: "RevieweeUsername",
                principalTable: "Users",
                principalColumn: "Username",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
