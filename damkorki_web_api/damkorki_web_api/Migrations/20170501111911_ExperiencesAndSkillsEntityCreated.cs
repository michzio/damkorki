using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DamkorkiWebApi.Migrations
{
    public partial class ExperiencesAndSkillsEntityCreated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "People",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Skype",
                table: "People",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "LessonOffers",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Experience",
                columns: table => new
                {
                    ExperienceId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: true),
                    EndYear = table.Column<int>(nullable: false),
                    StartYear = table.Column<int>(nullable: false),
                    TutorId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Experience", x => x.ExperienceId);
                    table.ForeignKey(
                        name: "FK_Experience_Tutors_TutorId",
                        column: x => x.TutorId,
                        principalTable: "Tutors",
                        principalColumn: "TutorId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Skill",
                columns: table => new
                {
                    SkillId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skill", x => x.SkillId);
                });

            migrationBuilder.CreateTable(
                name: "TutorSkill",
                columns: table => new
                {
                    TutorId = table.Column<int>(nullable: false),
                    SkillId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TutorSkill", x => new { x.TutorId, x.SkillId });
                    table.ForeignKey(
                        name: "FK_TutorSkill_Skill_SkillId",
                        column: x => x.SkillId,
                        principalTable: "Skill",
                        principalColumn: "SkillId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TutorSkill_Tutors_TutorId",
                        column: x => x.TutorId,
                        principalTable: "Tutors",
                        principalColumn: "TutorId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Experience_TutorId",
                table: "Experience",
                column: "TutorId");

            migrationBuilder.CreateIndex(
                name: "IX_TutorSkill_SkillId",
                table: "TutorSkill",
                column: "SkillId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Experience");

            migrationBuilder.DropTable(
                name: "TutorSkill");

            migrationBuilder.DropTable(
                name: "Skill");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "People");

            migrationBuilder.DropColumn(
                name: "Skype",
                table: "People");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "LessonOffers");
        }
    }
}
