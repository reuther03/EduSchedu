using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduSchedu.Modules.Schools.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddedDayOfWeekToScheduleItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<TimeOnly>(
                name: "Start",
                schema: "schools",
                table: "ScheduleItems",
                type: "time without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<TimeOnly>(
                name: "End",
                schema: "schools",
                table: "ScheduleItems",
                type: "time without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<string>(
                name: "Day",
                schema: "schools",
                table: "ScheduleItems",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Day",
                schema: "schools",
                table: "ScheduleItems");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Start",
                schema: "schools",
                table: "ScheduleItems",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(TimeOnly),
                oldType: "time without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "End",
                schema: "schools",
                table: "ScheduleItems",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(TimeOnly),
                oldType: "time without time zone");
        }
    }
}
