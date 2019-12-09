using Microsoft.EntityFrameworkCore.Migrations;

namespace StudyBuddyBackend.Migrations
{
    public partial class TeacherSubjectFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subjects_TeacherInfo_TeacherInfoUsername",
                table: "Subjects");

            migrationBuilder.DropIndex(
                name: "IX_Subjects_TeacherInfoUsername",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "TeacherInfoUsername",
                table: "Subjects");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TeacherInfoUsername",
                table: "Subjects",
                type: "character varying",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_TeacherInfoUsername",
                table: "Subjects",
                column: "TeacherInfoUsername");

            migrationBuilder.AddForeignKey(
                name: "FK_Subjects_TeacherInfo_TeacherInfoUsername",
                table: "Subjects",
                column: "TeacherInfoUsername",
                principalTable: "TeacherInfo",
                principalColumn: "Username",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
