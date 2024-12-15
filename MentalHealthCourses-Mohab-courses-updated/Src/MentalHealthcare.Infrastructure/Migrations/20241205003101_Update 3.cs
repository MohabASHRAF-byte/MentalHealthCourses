using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MentalHealthcare.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Update3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseMateriels_CourseLessons_CourseLessonId",
                table: "CourseMateriels");

            migrationBuilder.AlterColumn<int>(
                name: "CourseLessonId",
                table: "CourseMateriels",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseMateriels_CourseLessons_CourseLessonId",
                table: "CourseMateriels",
                column: "CourseLessonId",
                principalTable: "CourseLessons",
                principalColumn: "CourseLessonId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseMateriels_CourseLessons_CourseLessonId",
                table: "CourseMateriels");

            migrationBuilder.AlterColumn<int>(
                name: "CourseLessonId",
                table: "CourseMateriels",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseMateriels_CourseLessons_CourseLessonId",
                table: "CourseMateriels",
                column: "CourseLessonId",
                principalTable: "CourseLessons",
                principalColumn: "CourseLessonId");
        }
    }
}
