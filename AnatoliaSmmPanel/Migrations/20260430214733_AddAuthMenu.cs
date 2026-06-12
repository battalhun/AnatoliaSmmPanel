using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnatoliaSmmPanel.Migrations
{
    /// <inheritdoc />
    public partial class AddAuthMenu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AuthMenuId",
                table: "Menus",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AuthMenus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Controller = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Page = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Area = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsAdminOnly = table.Column<bool>(type: "bit", nullable: false),
                    OpenInNewTab = table.Column<bool>(type: "bit", nullable: false),
                    ParentId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthMenus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuthMenus_Menus_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Menus",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Menus_AuthMenuId",
                table: "Menus",
                column: "AuthMenuId");

            migrationBuilder.CreateIndex(
                name: "IX_AuthMenus_ParentId",
                table: "AuthMenus",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Menus_AuthMenus_AuthMenuId",
                table: "Menus",
                column: "AuthMenuId",
                principalTable: "AuthMenus",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Menus_AuthMenus_AuthMenuId",
                table: "Menus");

            migrationBuilder.DropTable(
                name: "AuthMenus");

            migrationBuilder.DropIndex(
                name: "IX_Menus_AuthMenuId",
                table: "Menus");

            migrationBuilder.DropColumn(
                name: "AuthMenuId",
                table: "Menus");
        }
    }
}
