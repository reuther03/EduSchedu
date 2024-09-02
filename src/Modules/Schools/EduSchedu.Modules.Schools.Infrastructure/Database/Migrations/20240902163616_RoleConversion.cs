using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduSchedu.Modules.Schools.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class RoleConversion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Role",
                schema: "schools",
                table: "SchoolUsers",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Role",
                schema: "schools",
                table: "SchoolUsers",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
