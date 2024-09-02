using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EduSchedu.Modules.Schools.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "schools");

            migrationBuilder.CreateTable(
                name: "LanguageProficiencies",
                schema: "schools",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Language = table.Column<string>(type: "text", nullable: false),
                    Lvl = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LanguageProficiencies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Schools",
                schema: "schools",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    City = table.Column<string>(type: "text", nullable: false),
                    Street = table.Column<string>(type: "text", nullable: false),
                    ZipCode = table.Column<string>(type: "text", nullable: false),
                    MapCoordinates = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    PrincipalId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schools", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Teachers",
                schema: "schools",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    FullName = table.Column<string>(type: "text", nullable: false),
                    Role = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teachers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Classes",
                schema: "schools",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    SchoolId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Classes_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalSchema: "schools",
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SchoolTeacherIds",
                schema: "schools",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SchoolId = table.Column<Guid>(type: "uuid", nullable: false),
                    TeacherId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolTeacherIds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SchoolTeacherIds_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalSchema: "schools",
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TeacherLanguageProficiencyIds",
                schema: "schools",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TeacherId = table.Column<Guid>(type: "uuid", nullable: false),
                    LanguageProficiencyId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherLanguageProficiencyIds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeacherLanguageProficiencyIds_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalSchema: "schools",
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClassLanguageProficiencyIds",
                schema: "schools",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClassId = table.Column<Guid>(type: "uuid", nullable: false),
                    LanguageProficiencyId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassLanguageProficiencyIds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClassLanguageProficiencyIds_Classes_ClassId",
                        column: x => x.ClassId,
                        principalSchema: "schools",
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Classes_SchoolId",
                schema: "schools",
                table: "Classes",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassLanguageProficiencyIds_ClassId",
                schema: "schools",
                table: "ClassLanguageProficiencyIds",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolTeacherIds_SchoolId",
                schema: "schools",
                table: "SchoolTeacherIds",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherLanguageProficiencyIds_TeacherId",
                schema: "schools",
                table: "TeacherLanguageProficiencyIds",
                column: "TeacherId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClassLanguageProficiencyIds",
                schema: "schools");

            migrationBuilder.DropTable(
                name: "LanguageProficiencies",
                schema: "schools");

            migrationBuilder.DropTable(
                name: "SchoolTeacherIds",
                schema: "schools");

            migrationBuilder.DropTable(
                name: "TeacherLanguageProficiencyIds",
                schema: "schools");

            migrationBuilder.DropTable(
                name: "Classes",
                schema: "schools");

            migrationBuilder.DropTable(
                name: "Teachers",
                schema: "schools");

            migrationBuilder.DropTable(
                name: "Schools",
                schema: "schools");
        }
    }
}
