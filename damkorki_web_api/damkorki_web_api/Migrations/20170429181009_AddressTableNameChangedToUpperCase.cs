using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DamkorkiWebApi.Migrations
{
    public partial class AddressTableNameChangedToUpperCase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_People_address_AddressId",
                table: "People");

            migrationBuilder.DropPrimaryKey(
                name: "PK_address",
                table: "address");

            migrationBuilder.RenameTable(
                name: "address",
                newName: "Address");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Address",
                table: "Address",
                column: "AddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_People_Address_AddressId",
                table: "People",
                column: "AddressId",
                principalTable: "Address",
                principalColumn: "AddressId",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_People_Address_AddressId",
                table: "People");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Address",
                table: "Address");

            migrationBuilder.RenameTable(
                name: "Address",
                newName: "address");

            migrationBuilder.AddPrimaryKey(
                name: "PK_address",
                table: "address",
                column: "AddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_People_address_AddressId",
                table: "People",
                column: "AddressId",
                principalTable: "address",
                principalColumn: "AddressId",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
