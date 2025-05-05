using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CorporatePortal.DL.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class AddedIsActiveToUserInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "UserInfos",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "UserInfos");
        }
    }
}
