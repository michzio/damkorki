using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace DamkorkiWebApi.Migrations
{
    public partial class AddressUpdatedToGlobal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HomeNumber",
                table: "Address");

            migrationBuilder.RenameColumn(
                name: "Voivodeship",
                table: "Address",
                newName: "Region");

            migrationBuilder.RenameColumn(
                name: "Street",
                table: "Address",
                newName: "AddressLine1");

            migrationBuilder.AddColumn<string>(
                name: "AddressLine2",
                table: "Address",
                maxLength: 150,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddressLine2",
                table: "Address");

            migrationBuilder.RenameColumn(
                name: "Region",
                table: "Address",
                newName: "Voivodeship");

            migrationBuilder.RenameColumn(
                name: "AddressLine1",
                table: "Address",
                newName: "Street");

            migrationBuilder.AddColumn<string>(
                name: "HomeNumber",
                table: "Address",
                maxLength: 10,
                nullable: false,
                defaultValue: "");
        }
    }
}
