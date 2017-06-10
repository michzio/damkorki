using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DamkorkiWebApi.Migrations
{
    public partial class AgeColumnChangedToBirthdateColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Age",
                table: "People");

            migrationBuilder.AddColumn<DateTime>(
                name: "Birthdate",
                table: "People",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Birthdate",
                table: "People");

            migrationBuilder.AddColumn<int>(
                name: "Age",
                table: "People",
                nullable: true);
        }
    }
}
