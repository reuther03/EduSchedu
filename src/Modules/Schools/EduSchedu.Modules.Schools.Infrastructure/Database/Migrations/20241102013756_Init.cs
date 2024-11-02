using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduSchedu.Modules.Schools.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_SchoolUsers_TeacherId",
                schema: "schools",
                table: "Schedules");

            migrationBuilder.DropForeignKey(
                name: "FK_TeacherLanguageProficiencyIds_SchoolUsers_TeacherId",
                schema: "schools",
                table: "TeacherLanguageProficiencyIds");

            migrationBuilder.CreateTable(
                name: "Headmaster",
                schema: "schools",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Headmaster", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                schema: "schools",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Students_SchoolUsers_Id",
                        column: x => x.Id,
                        principalSchema: "schools",
                        principalTable: "SchoolUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                name: "Headmaster",
                schema: "schools");

            migrationBuilder.DropTable(
                name: "Students",
                schema: "schools");

            migrationBuilder.DropTable(
                name: "Teacher",
                schema: "schools");

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_SchoolUsers_TeacherId",
                schema: "schools",
                table: "Schedules",
                column: "TeacherId",
                principalSchema: "schools",
                principalTable: "SchoolUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherLanguageProficiencyIds_SchoolUsers_TeacherId",
                schema: "schools",
                table: "TeacherLanguageProficiencyIds",
                column: "TeacherId",
                principalSchema: "schools",
                principalTable: "SchoolUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
