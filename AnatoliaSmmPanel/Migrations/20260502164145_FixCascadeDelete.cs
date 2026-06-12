using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnatoliaSmmPanel.Migrations
{
    /// <inheritdoc />
    public partial class FixCascadeDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuthMenus_Menus_ParentId",
                table: "AuthMenus");

            migrationBuilder.DropForeignKey(
                name: "FK_Menus_AuthMenus_AuthMenuId",
                table: "Menus");

            migrationBuilder.DropForeignKey(
                name: "FK_Menus_Menus_ParentId",
                table: "Menus");

            migrationBuilder.DropIndex(
                name: "IX_Menus_AuthMenuId",
                table: "Menus");

            migrationBuilder.DropIndex(
                name: "IX_Menus_ParentId",
                table: "Menus");

            migrationBuilder.DropIndex(
                name: "IX_AuthMenus_ParentId",
                table: "AuthMenus");

            migrationBuilder.DropColumn(
                name: "Action",
                table: "Menus");

            migrationBuilder.DropColumn(
                name: "Area",
                table: "Menus");

            migrationBuilder.DropColumn(
                name: "AuthMenuId",
                table: "Menus");

            migrationBuilder.DropColumn(
                name: "Controller",
                table: "Menus");

            migrationBuilder.DropColumn(
                name: "Page",
                table: "Menus");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "Menus");

            migrationBuilder.DropColumn(
                name: "PartialView",
                table: "Menus");

            migrationBuilder.DropColumn(
                name: "Url",
                table: "Menus");

            migrationBuilder.DropColumn(
                name: "Action",
                table: "AuthMenus");

            migrationBuilder.DropColumn(
                name: "Area",
                table: "AuthMenus");

            migrationBuilder.DropColumn(
                name: "Controller",
                table: "AuthMenus");

            migrationBuilder.DropColumn(
                name: "Page",
                table: "AuthMenus");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "AuthMenus");

            migrationBuilder.DropColumn(
                name: "PartialView",
                table: "AuthMenus");

            migrationBuilder.DropColumn(
                name: "Url",
                table: "AuthMenus");

            migrationBuilder.DropColumn(
                name: "Action",
                table: "AdminSubMenus");

            migrationBuilder.DropColumn(
                name: "Area",
                table: "AdminSubMenus");

            migrationBuilder.DropColumn(
                name: "Controller",
                table: "AdminSubMenus");

            migrationBuilder.DropColumn(
                name: "Page",
                table: "AdminSubMenus");

            migrationBuilder.DropColumn(
                name: "PartialView",
                table: "AdminSubMenus");

            migrationBuilder.DropColumn(
                name: "Url",
                table: "AdminSubMenus");

            migrationBuilder.DropColumn(
                name: "Action",
                table: "AdminMenus");

            migrationBuilder.DropColumn(
                name: "Area",
                table: "AdminMenus");

            migrationBuilder.DropColumn(
                name: "Controller",
                table: "AdminMenus");

            migrationBuilder.DropColumn(
                name: "Page",
                table: "AdminMenus");

            migrationBuilder.DropColumn(
                name: "PartialView",
                table: "AdminMenus");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Menus",
                newName: "NavigationTargetId");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "AuthMenus",
                newName: "NavigationTargetId");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "AdminSubMenus",
                newName: "NavigationTargetId");

            migrationBuilder.RenameColumn(
                name: "Url",
                table: "AdminMenus",
                newName: "RequiredRole");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "AdminMenus",
                newName: "NavigationTargetId");

            migrationBuilder.AddColumn<bool>(
                name: "IsAdminOnly",
                table: "AdminSubMenus",
                type: "bit",
                nullable: false,
                defaultValue: false);

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
                name: "AdminSubMenuPermissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdminSubMenuId = table.Column<int>(type: "int", nullable: false),
                    RoleName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AdminSubMenuId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminSubMenuPermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdminSubMenuPermissions_AdminSubMenus_AdminSubMenuId",
                        column: x => x.AdminSubMenuId,
                        principalTable: "AdminSubMenus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AdminSubMenuPermissions_AdminSubMenus_AdminSubMenuId1",
                        column: x => x.AdminSubMenuId1,
                        principalTable: "AdminSubMenus",
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
                    Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NavigationTargets", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Menus_NavigationTargetId",
                table: "Menus",
                column: "NavigationTargetId");

            migrationBuilder.CreateIndex(
                name: "IX_AuthMenus_NavigationTargetId",
                table: "AuthMenus",
                column: "NavigationTargetId");

            migrationBuilder.CreateIndex(
                name: "IX_AdminSubMenus_NavigationTargetId",
                table: "AdminSubMenus",
                column: "NavigationTargetId");

            migrationBuilder.CreateIndex(
                name: "IX_AdminMenus_NavigationTargetId",
                table: "AdminMenus",
                column: "NavigationTargetId");

            migrationBuilder.CreateIndex(
                name: "IX_AdminMenuPermissions_AdminMenuId",
                table: "AdminMenuPermissions",
                column: "AdminMenuId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AdminSubMenuPermissions_AdminSubMenuId",
                table: "AdminSubMenuPermissions",
                column: "AdminSubMenuId");

            migrationBuilder.CreateIndex(
                name: "IX_AdminSubMenuPermissions_AdminSubMenuId1",
                table: "AdminSubMenuPermissions",
                column: "AdminSubMenuId1",
                unique: true,
                filter: "[AdminSubMenuId1] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AuthMenuPermissions_AuthMenuId",
                table: "AuthMenuPermissions",
                column: "AuthMenuId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuPermissions_MenuId",
                table: "MenuPermissions",
                column: "MenuId");

            migrationBuilder.AddForeignKey(
                name: "FK_AdminMenus_NavigationTargets_NavigationTargetId",
                table: "AdminMenus",
                column: "NavigationTargetId",
                principalTable: "NavigationTargets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AdminSubMenus_NavigationTargets_NavigationTargetId",
                table: "AdminSubMenus",
                column: "NavigationTargetId",
                principalTable: "NavigationTargets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AuthMenus_NavigationTargets_NavigationTargetId",
                table: "AuthMenus",
                column: "NavigationTargetId",
                principalTable: "NavigationTargets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Menus_NavigationTargets_NavigationTargetId",
                table: "Menus",
                column: "NavigationTargetId",
                principalTable: "NavigationTargets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdminMenus_NavigationTargets_NavigationTargetId",
                table: "AdminMenus");

            migrationBuilder.DropForeignKey(
                name: "FK_AdminSubMenus_NavigationTargets_NavigationTargetId",
                table: "AdminSubMenus");

            migrationBuilder.DropForeignKey(
                name: "FK_AuthMenus_NavigationTargets_NavigationTargetId",
                table: "AuthMenus");

            migrationBuilder.DropForeignKey(
                name: "FK_Menus_NavigationTargets_NavigationTargetId",
                table: "Menus");

            migrationBuilder.DropTable(
                name: "AdminMenuPermissions");

            migrationBuilder.DropTable(
                name: "AdminSubMenuPermissions");

            migrationBuilder.DropTable(
                name: "AuthMenuPermissions");

            migrationBuilder.DropTable(
                name: "MenuPermissions");

            migrationBuilder.DropTable(
                name: "NavigationTargets");

            migrationBuilder.DropIndex(
                name: "IX_Menus_NavigationTargetId",
                table: "Menus");

            migrationBuilder.DropIndex(
                name: "IX_AuthMenus_NavigationTargetId",
                table: "AuthMenus");

            migrationBuilder.DropIndex(
                name: "IX_AdminSubMenus_NavigationTargetId",
                table: "AdminSubMenus");

            migrationBuilder.DropIndex(
                name: "IX_AdminMenus_NavigationTargetId",
                table: "AdminMenus");

            migrationBuilder.DropColumn(
                name: "IsAdminOnly",
                table: "AdminSubMenus");

            migrationBuilder.RenameColumn(
                name: "NavigationTargetId",
                table: "Menus",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "NavigationTargetId",
                table: "AuthMenus",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "NavigationTargetId",
                table: "AdminSubMenus",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "RequiredRole",
                table: "AdminMenus",
                newName: "Url");

            migrationBuilder.RenameColumn(
                name: "NavigationTargetId",
                table: "AdminMenus",
                newName: "Type");

            migrationBuilder.AddColumn<string>(
                name: "Action",
                table: "Menus",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Area",
                table: "Menus",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AuthMenuId",
                table: "Menus",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Controller",
                table: "Menus",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Page",
                table: "Menus",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "Menus",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PartialView",
                table: "Menus",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "Menus",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Action",
                table: "AuthMenus",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Area",
                table: "AuthMenus",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Controller",
                table: "AuthMenus",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Page",
                table: "AuthMenus",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "AuthMenus",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PartialView",
                table: "AuthMenus",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "AuthMenus",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Action",
                table: "AdminSubMenus",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Area",
                table: "AdminSubMenus",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Controller",
                table: "AdminSubMenus",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Page",
                table: "AdminSubMenus",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PartialView",
                table: "AdminSubMenus",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "AdminSubMenus",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Action",
                table: "AdminMenus",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Area",
                table: "AdminMenus",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Controller",
                table: "AdminMenus",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Page",
                table: "AdminMenus",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PartialView",
                table: "AdminMenus",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Menus_AuthMenuId",
                table: "Menus",
                column: "AuthMenuId");

            migrationBuilder.CreateIndex(
                name: "IX_Menus_ParentId",
                table: "Menus",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_AuthMenus_ParentId",
                table: "AuthMenus",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuthMenus_Menus_ParentId",
                table: "AuthMenus",
                column: "ParentId",
                principalTable: "Menus",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Menus_AuthMenus_AuthMenuId",
                table: "Menus",
                column: "AuthMenuId",
                principalTable: "AuthMenus",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Menus_Menus_ParentId",
                table: "Menus",
                column: "ParentId",
                principalTable: "Menus",
                principalColumn: "Id");
        }
    }
}
