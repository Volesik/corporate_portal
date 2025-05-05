using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CorporatePortal.DL.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class AddPositionToUserInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Position",
                table: "UserInfos",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Position",
                table: "UserInfos");
        }
    }
}
