using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CorporatePortal.DL.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class MakeUniqueIdUnique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_UserInfos_UniqueId",
                table: "UserInfos",
                column: "UniqueId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserInfos_UniqueId",
                table: "UserInfos");
        }
    }
}
