using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduSchedu.Modules.Schools.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class MovedScheduleToSchoolUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_Teachers_TeacherId",
                schema: "schools",
                table: "Schedules");

            migrationBuilder.DropTable(
                name: "SchoolApplication",
                schema: "schools");

            migrationBuilder.RenameColumn(
                name: "TeacherId",
                schema: "schools",
                table: "Schedules",
                newName: "SchoolUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Schedules_TeacherId",
                schema: "schools",
                table: "Schedules",
                newName: "IX_Schedules_SchoolUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_SchoolUsers_SchoolUserId",
                schema: "schools",
                table: "Schedules",
                column: "SchoolUserId",
                principalSchema: "schools",
                principalTable: "SchoolUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_SchoolUsers_SchoolUserId",
                schema: "schools",
                table: "Schedules");

            migrationBuilder.RenameColumn(
                name: "SchoolUserId",
                schema: "schools",
                table: "Schedules",
                newName: "TeacherId");

            migrationBuilder.RenameIndex(
                name: "IX_Schedules_SchoolUserId",
                schema: "schools",
                table: "Schedules",
                newName: "IX_Schedules_TeacherId");

            migrationBuilder.CreateTable(
                name: "SchoolApplication",
                schema: "schools",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Content = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    SchoolId = table.Column<Guid>(type: "uuid", nullable: true),
                    SchoolUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    SubmittedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolApplication", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SchoolApplication_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalSchema: "schools",
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SchoolApplication_SchoolId",
                schema: "schools",
                table: "SchoolApplication",
                column: "SchoolId");

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_Teachers_TeacherId",
                schema: "schools",
                table: "Schedules",
                column: "TeacherId",
                principalSchema: "schools",
                principalTable: "Teachers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
