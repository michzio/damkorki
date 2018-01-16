using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace DamkorkiWebApi.Migrations
{
    public partial class ProfilePictureIdChangedToProfilePhotoId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProfilePictureId",
                table: "ProfilePhoto",
                newName: "ProfilePhotoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProfilePhotoId",
                table: "ProfilePhoto",
                newName: "ProfilePictureId");
        }
    }
}
