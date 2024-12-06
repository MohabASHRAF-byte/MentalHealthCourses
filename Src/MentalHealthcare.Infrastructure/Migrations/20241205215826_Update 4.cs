using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MentalHealthcare.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Update4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseLessons_Courses_CourseId",
                table: "CourseLessons");

            migrationBuilder.DropTable(
                name: "CourseMateriels");

            migrationBuilder.DropIndex(
                name: "IX_CourseLessons_CourseId",
                table: "CourseLessons");

            migrationBuilder.RenameColumn(
                name: "CourseId",
                table: "CourseLessons",
                newName: "ContentType");

            migrationBuilder.AddColumn<bool>(
                name: "IsArchived",
                table: "Courses",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsFeatured",
                table: "Courses",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "AdminId",
                table: "CourseLessons",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "LessonBunnyName",
                table: "CourseLessons",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MaterielBunneyId",
                table: "CourseLessons",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "CourseLessons",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "CourseLessonResources",
                columns: table => new
                {
                    CourseLessonResourceId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ItemOrder = table.Column<int>(type: "integer", nullable: false),
                    Url = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    BunnyId = table.Column<string>(type: "text", nullable: false),
                    CourseLessonId = table.Column<int>(type: "integer", nullable: false),
                    AdminId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseLessonResources", x => x.CourseLessonResourceId);
                    table.ForeignKey(
                        name: "FK_CourseLessonResources_Admins_AdminId",
                        column: x => x.AdminId,
                        principalTable: "Admins",
                        principalColumn: "AdminId");
                    table.ForeignKey(
                        name: "FK_CourseLessonResources_CourseLessons_CourseLessonId",
                        column: x => x.CourseLessonId,
                        principalTable: "CourseLessons",
                        principalColumn: "CourseLessonId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseLessons_AdminId",
                table: "CourseLessons",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseLessonResources_AdminId",
                table: "CourseLessonResources",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseLessonResources_CourseLessonId",
                table: "CourseLessonResources",
                column: "CourseLessonId");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseLessons_Admins_AdminId",
                table: "CourseLessons",
                column: "AdminId",
                principalTable: "Admins",
                principalColumn: "AdminId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseLessons_Admins_AdminId",
                table: "CourseLessons");

            migrationBuilder.DropTable(
                name: "CourseLessonResources");

            migrationBuilder.DropIndex(
                name: "IX_CourseLessons_AdminId",
                table: "CourseLessons");

            migrationBuilder.DropColumn(
                name: "IsArchived",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "IsFeatured",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "AdminId",
                table: "CourseLessons");

            migrationBuilder.DropColumn(
                name: "LessonBunnyName",
                table: "CourseLessons");

            migrationBuilder.DropColumn(
                name: "MaterielBunneyId",
                table: "CourseLessons");

            migrationBuilder.DropColumn(
                name: "Url",
                table: "CourseLessons");

            migrationBuilder.RenameColumn(
                name: "ContentType",
                table: "CourseLessons",
                newName: "CourseId");

            migrationBuilder.CreateTable(
                name: "CourseMateriels",
                columns: table => new
                {
                    CourseMaterielId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AdminId = table.Column<int>(type: "integer", nullable: false),
                    CourseId = table.Column<int>(type: "integer", nullable: false),
                    CourseLessonId = table.Column<int>(type: "integer", nullable: false),
                    CourseSectionId = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    IsVideo = table.Column<bool>(type: "boolean", nullable: false),
                    ItemOrder = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Url = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseMateriels", x => x.CourseMaterielId);
                    table.ForeignKey(
                        name: "FK_CourseMateriels_Admins_AdminId",
                        column: x => x.AdminId,
                        principalTable: "Admins",
                        principalColumn: "AdminId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseMateriels_CourseLessons_CourseLessonId",
                        column: x => x.CourseLessonId,
                        principalTable: "CourseLessons",
                        principalColumn: "CourseLessonId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseMateriels_CourseSections_CourseSectionId",
                        column: x => x.CourseSectionId,
                        principalTable: "CourseSections",
                        principalColumn: "CourseSectionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseMateriels_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "CourseId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseLessons_CourseId",
                table: "CourseLessons",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseMateriels_AdminId",
                table: "CourseMateriels",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseMateriels_CourseId",
                table: "CourseMateriels",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseMateriels_CourseLessonId",
                table: "CourseMateriels",
                column: "CourseLessonId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseMateriels_CourseSectionId",
                table: "CourseMateriels",
                column: "CourseSectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseLessons_Courses_CourseId",
                table: "CourseLessons",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "CourseId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
