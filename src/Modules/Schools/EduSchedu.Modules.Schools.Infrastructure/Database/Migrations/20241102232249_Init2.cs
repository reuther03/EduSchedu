using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduSchedu.Modules.Schools.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Init2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_Teacher_TeacherId",
                schema: "schools",
                table: "Schedules");

            migrationBuilder.DropForeignKey(
                name: "FK_TeacherLanguageProficiencyIds_Teacher_TeacherId",
                schema: "schools",
                table: "TeacherLanguageProficiencyIds");

            migrationBuilder.DropTable(
                name: "Teacher",
                schema: "schools");

            migrationBuilder.RenameTable(
                name: "TeacherLanguageProficiencyIds",
                schema: "schools",
                newName: "TeacherLanguageProficiencyIds");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "TeacherLanguageProficiencyIds",
                newName: "TeacherLanguageProficiencyIds",
                newSchema: "schools");

            migrationBuilder.CreateTable(
                name: "Teacher",
                schema: "schools",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teacher", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Teacher_SchoolUsers_Id",
                        column: x => x.Id,
                        principalSchema: "schools",
                        principalTable: "SchoolUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_Teacher_TeacherId",
                schema: "schools",
                table: "Schedules",
                column: "TeacherId",
                principalSchema: "schools",
                principalTable: "Teacher",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherLanguageProficiencyIds_Teacher_TeacherId",
                schema: "schools",
                table: "TeacherLanguageProficiencyIds",
                column: "TeacherId",
                principalSchema: "schools",
                principalTable: "Teacher",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
