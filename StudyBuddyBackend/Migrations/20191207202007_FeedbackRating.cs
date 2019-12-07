using Microsoft.EntityFrameworkCore.Migrations;

namespace StudyBuddyBackend.Migrations
{
    public partial class FeedbackRating : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Rating",
                table: "Feedback",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Feedback");
        }
    }
}
