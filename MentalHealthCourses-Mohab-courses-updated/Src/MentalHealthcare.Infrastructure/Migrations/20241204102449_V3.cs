using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MentalHealthcare.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class V3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CourseLessonId",
                table: "VideoUploads",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CourseSectionId",
                table: "VideoUploads",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_VideoUploads_CourseLessonId",
                table: "VideoUploads",
                column: "CourseLessonId");

            migrationBuilder.CreateIndex(
                name: "IX_VideoUploads_CourseSectionId",
                table: "VideoUploads",
                column: "CourseSectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_VideoUploads_CourseLessons_CourseLessonId",
                table: "VideoUploads",
                column: "CourseLessonId",
                principalTable: "CourseLessons",
                principalColumn: "CourseLessonId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VideoUploads_CourseSections_CourseSectionId",
                table: "VideoUploads",
                column: "CourseSectionId",
                principalTable: "CourseSections",
                principalColumn: "CourseSectionId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VideoUploads_CourseLessons_CourseLessonId",
                table: "VideoUploads");

            migrationBuilder.DropForeignKey(
                name: "FK_VideoUploads_CourseSections_CourseSectionId",
                table: "VideoUploads");

            migrationBuilder.DropIndex(
                name: "IX_VideoUploads_CourseLessonId",
                table: "VideoUploads");

            migrationBuilder.DropIndex(
                name: "IX_VideoUploads_CourseSectionId",
                table: "VideoUploads");

            migrationBuilder.DropColumn(
                name: "CourseLessonId",
                table: "VideoUploads");

            migrationBuilder.DropColumn(
                name: "CourseSectionId",
                table: "VideoUploads");
        }
    }
}
