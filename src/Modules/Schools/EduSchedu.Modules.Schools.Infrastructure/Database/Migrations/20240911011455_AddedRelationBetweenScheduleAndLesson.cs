using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EduSchedu.Modules.Schools.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddedRelationBetweenScheduleAndLesson : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ScheduleLessonIds",
                schema: "schools",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ScheduleId = table.Column<Guid>(type: "uuid", nullable: false),
                    LessonId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleLessonIds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScheduleLessonIds_Schedules_ScheduleId",
                        column: x => x.ScheduleId,
                        principalSchema: "schools",
                        principalTable: "Schedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleLessonIds_ScheduleId",
                schema: "schools",
                table: "ScheduleLessonIds",
                column: "ScheduleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ScheduleLessonIds",
                schema: "schools");
        }
    }
}
