using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EduSchedu.Modules.Schools.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class RefactoredLesson : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LessonTeacherIds",
                schema: "schools");

            migrationBuilder.AddColumn<Guid>(
                name: "AssignedTeacherId",
                schema: "schools",
                table: "Lesson",
                type: "uuid",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssignedTeacherId",
                schema: "schools",
                table: "Lesson");

            migrationBuilder.CreateTable(
                name: "LessonTeacherIds",
                schema: "schools",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LessonId = table.Column<Guid>(type: "uuid", nullable: false),
                    TeacherId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LessonTeacherIds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LessonTeacherIds_Lesson_LessonId",
                        column: x => x.LessonId,
                        principalSchema: "schools",
                        principalTable: "Lesson",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LessonTeacherIds_LessonId",
                schema: "schools",
                table: "LessonTeacherIds",
                column: "LessonId");
        }
    }
}
