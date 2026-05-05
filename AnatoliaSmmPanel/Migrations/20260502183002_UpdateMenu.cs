using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnatoliaSmmPanel.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMenu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MenuPermissions_MenuId",
                table: "MenuPermissions");

            migrationBuilder.DropIndex(
                name: "IX_AuthMenuPermissions_AuthMenuId",
                table: "AuthMenuPermissions");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "NavigationTargets",
                newName: "MenuConnect");

            migrationBuilder.CreateIndex(
                name: "IX_MenuPermissions_MenuId",
                table: "MenuPermissions",
                column: "MenuId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuthMenuPermissions_AuthMenuId",
                table: "AuthMenuPermissions",
                column: "AuthMenuId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MenuPermissions_MenuId",
                table: "MenuPermissions");

            migrationBuilder.DropIndex(
                name: "IX_AuthMenuPermissions_AuthMenuId",
                table: "AuthMenuPermissions");

            migrationBuilder.RenameColumn(
                name: "MenuConnect",
                table: "NavigationTargets",
                newName: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_MenuPermissions_MenuId",
                table: "MenuPermissions",
                column: "MenuId");

            migrationBuilder.CreateIndex(
                name: "IX_AuthMenuPermissions_AuthMenuId",
                table: "AuthMenuPermissions",
                column: "AuthMenuId");
        }
    }
}
