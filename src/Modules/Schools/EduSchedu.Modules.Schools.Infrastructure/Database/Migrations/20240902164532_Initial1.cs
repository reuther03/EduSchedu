using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduSchedu.Modules.Schools.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Initial1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lesson_SchoolUsers_TeacherId",
                schema: "schools",
                table: "Lesson");

            migrationBuilder.RenameColumn(
                name: "TeacherId",
                schema: "schools",
                table: "Lesson",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Lesson_TeacherId",
                schema: "schools",
                table: "Lesson",
                newName: "IX_Lesson_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Lesson_SchoolUsers_UserId",
                schema: "schools",
                table: "Lesson",
                column: "UserId",
                principalSchema: "schools",
                principalTable: "SchoolUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lesson_SchoolUsers_UserId",
                schema: "schools",
                table: "Lesson");

            migrationBuilder.RenameColumn(
                name: "UserId",
                schema: "schools",
                table: "Lesson",
                newName: "TeacherId");

            migrationBuilder.RenameIndex(
                name: "IX_Lesson_UserId",
                schema: "schools",
                table: "Lesson",
                newName: "IX_Lesson_TeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_Lesson_SchoolUsers_TeacherId",
                schema: "schools",
                table: "Lesson",
                column: "TeacherId",
                principalSchema: "schools",
                principalTable: "SchoolUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
