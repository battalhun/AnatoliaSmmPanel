using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnatoliaSmmPanel.Data.Migrations.Admin
{
    /// <inheritdoc />
    public partial class updateservices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_externalServiceInfos_SmmServices_SmmServiceId",
                table: "externalServiceInfos");

            migrationBuilder.DropIndex(
                name: "IX_externalServiceInfos_SmmServiceId",
                table: "externalServiceInfos");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "SmmServices");

            migrationBuilder.DropColumn(
                name: "SmmServiceId",
                table: "externalServiceInfos");

            migrationBuilder.RenameColumn(
                name: "ExternalServiceId",
                table: "SmmServices",
                newName: "externalServiceInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_SmmServices_externalServiceInfoId",
                table: "SmmServices",
                column: "externalServiceInfoId");

            migrationBuilder.AddForeignKey(
                name: "FK_SmmServices_externalServiceInfos_externalServiceInfoId",
                table: "SmmServices",
                column: "externalServiceInfoId",
                principalTable: "externalServiceInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SmmServices_externalServiceInfos_externalServiceInfoId",
                table: "SmmServices");

            migrationBuilder.DropIndex(
                name: "IX_SmmServices_externalServiceInfoId",
                table: "SmmServices");

            migrationBuilder.RenameColumn(
                name: "externalServiceInfoId",
                table: "SmmServices",
                newName: "ExternalServiceId");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "SmmServices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SmmServiceId",
                table: "externalServiceInfos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_externalServiceInfos_SmmServiceId",
                table: "externalServiceInfos",
                column: "SmmServiceId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_externalServiceInfos_SmmServices_SmmServiceId",
                table: "externalServiceInfos",
                column: "SmmServiceId",
                principalTable: "SmmServices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
