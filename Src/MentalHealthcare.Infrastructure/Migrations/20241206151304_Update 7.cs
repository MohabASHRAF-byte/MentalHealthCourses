using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MentalHealthcare.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Update7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VideoUploads_CourseLessons_CourseLessonId",
                table: "VideoUploads");

            migrationBuilder.DropIndex(
                name: "IX_VideoUploads_CourseLessonId",
                table: "VideoUploads");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "VideoUploads");

            migrationBuilder.DropColumn(
                name: "CourseLessonId",
                table: "VideoUploads");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CourseId",
                table: "VideoUploads",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CourseLessonId",
                table: "VideoUploads",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_VideoUploads_CourseLessonId",
                table: "VideoUploads",
                column: "CourseLessonId");

            migrationBuilder.AddForeignKey(
                name: "FK_VideoUploads_CourseLessons_CourseLessonId",
                table: "VideoUploads",
                column: "CourseLessonId",
                principalTable: "CourseLessons",
                principalColumn: "CourseLessonId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
