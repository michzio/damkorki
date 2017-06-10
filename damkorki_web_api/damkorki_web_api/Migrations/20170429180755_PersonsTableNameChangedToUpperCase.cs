using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DamkorkiWebApi.Migrations
{
    public partial class PersonsTableNameChangedToUpperCase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Learners_people_PersonId",
                table: "Learners");

            migrationBuilder.DropForeignKey(
                name: "FK_people_address_AddressId",
                table: "people");

            migrationBuilder.DropForeignKey(
                name: "FK_people_AspNetUsers_UserId",
                table: "people");

            migrationBuilder.DropForeignKey(
                name: "FK_Tutors_people_PersonId",
                table: "Tutors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_people",
                table: "people");

            migrationBuilder.RenameTable(
                name: "people",
                newName: "People");

            migrationBuilder.RenameIndex(
                name: "IX_people_UserId",
                table: "People",
                newName: "IX_People_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_people_AddressId",
                table: "People",
                newName: "IX_People_AddressId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_People",
                table: "People",
                column: "PersonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Learners_People_PersonId",
                table: "Learners",
                column: "PersonId",
                principalTable: "People",
                principalColumn: "PersonId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_People_address_AddressId",
                table: "People",
                column: "AddressId",
                principalTable: "address",
                principalColumn: "AddressId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_People_AspNetUsers_UserId",
                table: "People",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tutors_People_PersonId",
                table: "Tutors",
                column: "PersonId",
                principalTable: "People",
                principalColumn: "PersonId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Learners_People_PersonId",
                table: "Learners");

            migrationBuilder.DropForeignKey(
                name: "FK_People_address_AddressId",
                table: "People");

            migrationBuilder.DropForeignKey(
                name: "FK_People_AspNetUsers_UserId",
                table: "People");

            migrationBuilder.DropForeignKey(
                name: "FK_Tutors_People_PersonId",
                table: "Tutors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_People",
                table: "People");

            migrationBuilder.RenameTable(
                name: "People",
                newName: "people");

            migrationBuilder.RenameIndex(
                name: "IX_People_UserId",
                table: "people",
                newName: "IX_people_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_People_AddressId",
                table: "people",
                newName: "IX_people_AddressId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_people",
                table: "people",
                column: "PersonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Learners_people_PersonId",
                table: "Learners",
                column: "PersonId",
                principalTable: "people",
                principalColumn: "PersonId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_people_address_AddressId",
                table: "people",
                column: "AddressId",
                principalTable: "address",
                principalColumn: "AddressId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_people_AspNetUsers_UserId",
                table: "people",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tutors_people_PersonId",
                table: "Tutors",
                column: "PersonId",
                principalTable: "people",
                principalColumn: "PersonId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
