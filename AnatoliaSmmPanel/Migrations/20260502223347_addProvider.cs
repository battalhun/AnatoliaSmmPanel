using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnatoliaSmmPanel.Migrations
{
    /// <inheritdoc />
    public partial class addProvider : Migration
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

            migrationBuilder.DropIndex(
                name: "IX_AdminSubMenuPermissions_AdminSubMenuId1",
                table: "AdminSubMenuPermissions");

            migrationBuilder.DropIndex(
                name: "IX_AdminMenuPermissions_AdminMenuId",
                table: "AdminMenuPermissions");

            migrationBuilder.CreateTable(
                name: "Providers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Alias = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    ApiUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ApiKey = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    LowBalanceNotificationEnabled = table.Column<bool>(type: "bit", nullable: true),
                    BalanceLimit = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    CurrentBalance = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    Currency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    LastCheckAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastSyncAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Providers", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MenuPermissions_MenuId",
                table: "MenuPermissions",
                column: "MenuId");

            migrationBuilder.CreateIndex(
                name: "IX_AuthMenuPermissions_AuthMenuId",
                table: "AuthMenuPermissions",
                column: "AuthMenuId");

            migrationBuilder.CreateIndex(
                name: "IX_AdminSubMenuPermissions_AdminSubMenuId1",
                table: "AdminSubMenuPermissions",
                column: "AdminSubMenuId1");

            migrationBuilder.CreateIndex(
                name: "IX_AdminMenuPermissions_AdminMenuId",
                table: "AdminMenuPermissions",
                column: "AdminMenuId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Providers");

            migrationBuilder.DropIndex(
                name: "IX_MenuPermissions_MenuId",
                table: "MenuPermissions");

            migrationBuilder.DropIndex(
                name: "IX_AuthMenuPermissions_AuthMenuId",
                table: "AuthMenuPermissions");

            migrationBuilder.DropIndex(
                name: "IX_AdminSubMenuPermissions_AdminSubMenuId1",
                table: "AdminSubMenuPermissions");

            migrationBuilder.DropIndex(
                name: "IX_AdminMenuPermissions_AdminMenuId",
                table: "AdminMenuPermissions");

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

            migrationBuilder.CreateIndex(
                name: "IX_AdminSubMenuPermissions_AdminSubMenuId1",
                table: "AdminSubMenuPermissions",
                column: "AdminSubMenuId1",
                unique: true,
                filter: "[AdminSubMenuId1] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AdminMenuPermissions_AdminMenuId",
                table: "AdminMenuPermissions",
                column: "AdminMenuId",
                unique: true);
        }
    }
}
