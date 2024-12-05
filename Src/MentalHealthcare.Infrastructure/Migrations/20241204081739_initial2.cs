using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MentalHealthcare.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class initial2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CourseLessonId",
                table: "CourseMateriels",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CourseSectionId",
                table: "CourseMateriels",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CourseLessons",
                columns: table => new
                {
                    CourseLessonId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LessonName = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Order = table.Column<int>(type: "integer", nullable: false),
                    CourseSectionId = table.Column<int>(type: "integer", nullable: false),
                    CourseId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseLessons", x => x.CourseLessonId);
                    table.ForeignKey(
                        name: "FK_CourseLessons_CourseSections_CourseSectionId",
                        column: x => x.CourseSectionId,
                        principalTable: "CourseSections",
                        principalColumn: "CourseSectionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseLessons_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "CourseId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseMateriels_CourseLessonId",
                table: "CourseMateriels",
                column: "CourseLessonId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseMateriels_CourseSectionId",
                table: "CourseMateriels",
                column: "CourseSectionId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseLessons_CourseId",
                table: "CourseLessons",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseLessons_CourseSectionId",
                table: "CourseLessons",
                column: "CourseSectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseMateriels_CourseLessons_CourseLessonId",
                table: "CourseMateriels",
                column: "CourseLessonId",
                principalTable: "CourseLessons",
                principalColumn: "CourseLessonId");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseMateriels_CourseSections_CourseSectionId",
                table: "CourseMateriels",
                column: "CourseSectionId",
                principalTable: "CourseSections",
                principalColumn: "CourseSectionId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseMateriels_CourseLessons_CourseLessonId",
                table: "CourseMateriels");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseMateriels_CourseSections_CourseSectionId",
                table: "CourseMateriels");

            migrationBuilder.DropTable(
                name: "CourseLessons");

            migrationBuilder.DropIndex(
                name: "IX_CourseMateriels_CourseLessonId",
                table: "CourseMateriels");

            migrationBuilder.DropIndex(
                name: "IX_CourseMateriels_CourseSectionId",
                table: "CourseMateriels");

            migrationBuilder.DropColumn(
                name: "CourseLessonId",
                table: "CourseMateriels");

            migrationBuilder.DropColumn(
                name: "CourseSectionId",
                table: "CourseMateriels");
        }
    }
}
