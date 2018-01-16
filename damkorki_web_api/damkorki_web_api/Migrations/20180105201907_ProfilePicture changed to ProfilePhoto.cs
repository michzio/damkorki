using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace DamkorkiWebApi.Migrations
{
    public partial class ProfilePicturechangedtoProfilePhoto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProfilePicture");

            migrationBuilder.CreateTable(
                name: "ProfilePhoto",
                columns: table => new
                {
                    ProfilePictureId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Caption = table.Column<string>(nullable: true),
                    FileName = table.Column<string>(nullable: false),
                    IsProfilePhoto = table.Column<bool>(nullable: false, defaultValue: false),
                    PersonId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfilePhoto", x => x.ProfilePictureId);
                    table.ForeignKey(
                        name: "FK_ProfilePhoto_People_PersonId",
                        column: x => x.PersonId,
                        principalTable: "People",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProfilePhoto_PersonId",
                table: "ProfilePhoto",
                column: "PersonId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProfilePhoto");

            migrationBuilder.CreateTable(
                name: "ProfilePicture",
                columns: table => new
                {
                    ProfilePictureId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Caption = table.Column<string>(nullable: true),
                    FileName = table.Column<string>(nullable: false),
                    IsProfilePicture = table.Column<bool>(nullable: false, defaultValue: false),
                    PersonId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfilePicture", x => x.ProfilePictureId);
                    table.ForeignKey(
                        name: "FK_ProfilePicture_People_PersonId",
                        column: x => x.PersonId,
                        principalTable: "People",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProfilePicture_PersonId",
                table: "ProfilePicture",
                column: "PersonId");
        }
    }
}
