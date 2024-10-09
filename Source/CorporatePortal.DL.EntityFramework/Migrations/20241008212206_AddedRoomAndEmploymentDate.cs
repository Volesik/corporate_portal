using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CorporatePortal.DL.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class AddedRoomAndEmploymentDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "EmploymentDate",
                table: "UserInfos",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Room",
                table: "UserInfos",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmploymentDate",
                table: "UserInfos");

            migrationBuilder.DropColumn(
                name: "Room",
                table: "UserInfos");
        }
    }
}
