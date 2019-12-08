using Microsoft.EntityFrameworkCore.Migrations;

namespace StudyBuddyBackend.Migrations
{
    public partial class TeacherInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserSubjects");

            migrationBuilder.AddColumn<string>(
                name: "TeacherInfoUsername",
                table: "Subjects",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TeacherInfo",
                columns: table => new
                {
                    Username = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherInfo", x => x.Username);
                    table.ForeignKey(
                        name: "FK_TeacherInfo_Users_Username",
                        column: x => x.Username,
                        principalTable: "Users",
                        principalColumn: "Username",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TeacherSubjects",
                columns: table => new
                {
                    SubjectName = table.Column<string>(nullable: false),
                    Username = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherSubjects", x => new { x.SubjectName, x.Username });
                    table.ForeignKey(
                        name: "FK_TeacherSubjects_Subjects_SubjectName",
                        column: x => x.SubjectName,
                        principalTable: "Subjects",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeacherSubjects_TeacherInfo_Username",
                        column: x => x.Username,
                        principalTable: "TeacherInfo",
                        principalColumn: "Username",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_TeacherInfoUsername",
                table: "Subjects",
                column: "TeacherInfoUsername");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherSubjects_Username",
                table: "TeacherSubjects",
                column: "Username");

            migrationBuilder.AddForeignKey(
                name: "FK_Subjects_TeacherInfo_TeacherInfoUsername",
                table: "Subjects",
                column: "TeacherInfoUsername",
                principalTable: "TeacherInfo",
                principalColumn: "Username",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subjects_TeacherInfo_TeacherInfoUsername",
                table: "Subjects");

            migrationBuilder.DropTable(
                name: "TeacherSubjects");

            migrationBuilder.DropTable(
                name: "TeacherInfo");

            migrationBuilder.DropIndex(
                name: "IX_Subjects_TeacherInfoUsername",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "TeacherInfoUsername",
                table: "Subjects");

            migrationBuilder.CreateTable(
                name: "UserSubjects",
                columns: table => new
                {
                    SubjectName = table.Column<string>(type: "character varying", nullable: false),
                    Username = table.Column<string>(type: "character varying", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSubjects", x => new { x.SubjectName, x.Username });
                    table.ForeignKey(
                        name: "FK_UserSubjects_Subjects_SubjectName",
                        column: x => x.SubjectName,
                        principalTable: "Subjects",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserSubjects_Users_Username",
                        column: x => x.Username,
                        principalTable: "Users",
                        principalColumn: "Username",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserSubjects_Username",
                table: "UserSubjects",
                column: "Username");
        }
    }
}
