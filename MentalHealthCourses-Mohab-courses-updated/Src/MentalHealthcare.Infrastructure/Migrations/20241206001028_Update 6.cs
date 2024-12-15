using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MentalHealthcare.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Update6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BunnyPath",
                table: "CourseLessonResources",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BunnyPath",
                table: "CourseLessonResources");
        }
    }
}
