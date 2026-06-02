using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnatoliaSmmPanel.Data.Migrations.Home
{
    /// <inheritdoc />
    public partial class AddMenu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NavigationTargets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Controller = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Page = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Area = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PartialView = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MenuConnect = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NavigationTargets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AdminMenus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NavigationTargetId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    RequiredRole = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsAdminOnly = table.Column<bool>(type: "bit", nullable: false),
                    OpenInNewTab = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminMenus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdminMenus_NavigationTargets_NavigationTargetId",
                        column: x => x.NavigationTargetId,
                        principalTable: "NavigationTargets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AuthMenus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NavigationTargetId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsAdminOnly = table.Column<bool>(type: "bit", nullable: false),
                    OpenInNewTab = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthMenus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuthMenus_NavigationTargets_NavigationTargetId",
                        column: x => x.NavigationTargetId,
                        principalTable: "NavigationTargets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Menus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NavigationTargetId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsAdminOnly = table.Column<bool>(type: "bit", nullable: false),
                    OpenInNewTab = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Menus_NavigationTargets_NavigationTargetId",
                        column: x => x.NavigationTargetId,
                        principalTable: "NavigationTargets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AdminMenuPermissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdminMenuId = table.Column<int>(type: "int", nullable: false),
                    RoleName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminMenuPermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdminMenuPermissions_AdminMenus_AdminMenuId",
                        column: x => x.AdminMenuId,
                        principalTable: "AdminMenus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AdminSubMenus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NavigationTargetId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsAdminOnly = table.Column<bool>(type: "bit", nullable: false),
                    OpenInNewTab = table.Column<bool>(type: "bit", nullable: false),
                    AdminMenuId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminSubMenus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdminSubMenus_AdminMenus_AdminMenuId",
                        column: x => x.AdminMenuId,
                        principalTable: "AdminMenus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AdminSubMenus_NavigationTargets_NavigationTargetId",
                        column: x => x.NavigationTargetId,
                        principalTable: "NavigationTargets",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AuthMenuPermissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuthMenuId = table.Column<int>(type: "int", nullable: false),
                    RoleName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthMenuPermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuthMenuPermissions_AuthMenus_AuthMenuId",
                        column: x => x.AuthMenuId,
                        principalTable: "AuthMenus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MenuPermissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MenuId = table.Column<int>(type: "int", nullable: false),
                    RoleName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuPermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MenuPermissions_Menus_MenuId",
                        column: x => x.MenuId,
                        principalTable: "Menus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AdminSubMenuPermissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdminSubMenuId = table.Column<int>(type: "int", nullable: false),
                    RoleName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminSubMenuPermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdminSubMenuPermissions_AdminSubMenus_AdminSubMenuId",
                        column: x => x.AdminSubMenuId,
                        principalTable: "AdminSubMenus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdminMenuPermissions_AdminMenuId",
                table: "AdminMenuPermissions",
                column: "AdminMenuId");

            migrationBuilder.CreateIndex(
                name: "IX_AdminMenus_NavigationTargetId",
                table: "AdminMenus",
                column: "NavigationTargetId");

            migrationBuilder.CreateIndex(
                name: "IX_AdminSubMenuPermissions_AdminSubMenuId",
                table: "AdminSubMenuPermissions",
                column: "AdminSubMenuId");

            migrationBuilder.CreateIndex(
                name: "IX_AdminSubMenus_AdminMenuId",
                table: "AdminSubMenus",
                column: "AdminMenuId");

            migrationBuilder.CreateIndex(
                name: "IX_AdminSubMenus_NavigationTargetId",
                table: "AdminSubMenus",
                column: "NavigationTargetId");

            migrationBuilder.CreateIndex(
                name: "IX_AuthMenuPermissions_AuthMenuId",
                table: "AuthMenuPermissions",
                column: "AuthMenuId");

            migrationBuilder.CreateIndex(
                name: "IX_AuthMenus_NavigationTargetId",
                table: "AuthMenus",
                column: "NavigationTargetId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuPermissions_MenuId",
                table: "MenuPermissions",
                column: "MenuId");

            migrationBuilder.CreateIndex(
                name: "IX_Menus_NavigationTargetId",
                table: "Menus",
                column: "NavigationTargetId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdminMenuPermissions");

            migrationBuilder.DropTable(
                name: "AdminSubMenuPermissions");

            migrationBuilder.DropTable(
                name: "AuthMenuPermissions");

            migrationBuilder.DropTable(
                name: "MenuPermissions");

            migrationBuilder.DropTable(
                name: "AdminSubMenus");

            migrationBuilder.DropTable(
                name: "AuthMenus");

            migrationBuilder.DropTable(
                name: "Menus");

            migrationBuilder.DropTable(
                name: "AdminMenus");

            migrationBuilder.DropTable(
                name: "NavigationTargets");
        }
    }
}
