using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnatoliaSmmPanel.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAdminSubMenu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAdminOnly",
                table: "AdminSubMenus");

            migrationBuilder.AddColumn<string>(
                name: "PartialView",
                table: "Menus",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PartialView",
                table: "AuthMenus",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AdminMenuId",
                table: "AdminSubMenus",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PartialView",
                table: "AdminSubMenus",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PartialView",
                table: "AdminMenus",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AdminSubMenus_AdminMenuId",
                table: "AdminSubMenus",
                column: "AdminMenuId");

            migrationBuilder.AddForeignKey(
                name: "FK_AdminSubMenus_AdminMenus_AdminMenuId",
                table: "AdminSubMenus",
                column: "AdminMenuId",
                principalTable: "AdminMenus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdminSubMenus_AdminMenus_AdminMenuId",
                table: "AdminSubMenus");

            migrationBuilder.DropIndex(
                name: "IX_AdminSubMenus_AdminMenuId",
                table: "AdminSubMenus");

            migrationBuilder.DropColumn(
                name: "PartialView",
                table: "Menus");

            migrationBuilder.DropColumn(
                name: "PartialView",
                table: "AuthMenus");

            migrationBuilder.DropColumn(
                name: "AdminMenuId",
                table: "AdminSubMenus");

            migrationBuilder.DropColumn(
                name: "PartialView",
                table: "AdminSubMenus");

            migrationBuilder.DropColumn(
                name: "PartialView",
                table: "AdminMenus");

            migrationBuilder.AddColumn<bool>(
                name: "IsAdminOnly",
                table: "AdminSubMenus",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
