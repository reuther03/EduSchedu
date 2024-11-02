using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

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
                name: "Headmaster",
                schema: "schools");

            migrationBuilder.DropTable(
                name: "Students",
                schema: "schools");

            migrationBuilder.DropTable(
                name: "Teacher",
                schema: "schools");

            migrationBuilder.CreateTable(
                name: "Grade",
                schema: "schools",
                columns: table => new
                {
                    StudentId = table.Column<Guid>(type: "uuid", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Grade = table.Column<float>(type: "real", nullable: false),
                    Percentage = table.Column<int>(type: "integer", nullable: true),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grade", x => new { x.StudentId, x.Id });
                    table.ForeignKey(
                        name: "FK_Grade_SchoolUsers_StudentId",
                        column: x => x.StudentId,
                        principalSchema: "schools",
                        principalTable: "SchoolUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_SchoolUsers_TeacherId",
                schema: "schools",
                table: "Schedules");

            migrationBuilder.DropForeignKey(
                name: "FK_TeacherLanguageProficiencyIds_SchoolUsers_TeacherId",
                schema: "schools",
                table: "TeacherLanguageProficiencyIds");

            migrationBuilder.DropTable(
                name: "Grade",
                schema: "schools");

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
    }
}
