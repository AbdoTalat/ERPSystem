using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERPSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDefaultBranchIdToUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Branches_AspNetUsers_CreatedById",
                table: "Branches");

            migrationBuilder.DropForeignKey(
                name: "FK_Branches_AspNetUsers_LastUpdatedById",
                table: "Branches");

            migrationBuilder.DropIndex(
                name: "IX_Branches_CreatedById",
                table: "Branches");

            migrationBuilder.DropIndex(
                name: "IX_Branches_LastUpdatedById",
                table: "Branches");

            migrationBuilder.AddColumn<int>(
                name: "DefaultBranchId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_DefaultBranchId",
                table: "AspNetUsers",
                column: "DefaultBranchId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Branches_DefaultBranchId",
                table: "AspNetUsers",
                column: "DefaultBranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Branches_DefaultBranchId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_DefaultBranchId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DefaultBranchId",
                table: "AspNetUsers");

            migrationBuilder.CreateIndex(
                name: "IX_Branches_CreatedById",
                table: "Branches",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Branches_LastUpdatedById",
                table: "Branches",
                column: "LastUpdatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Branches_AspNetUsers_CreatedById",
                table: "Branches",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Branches_AspNetUsers_LastUpdatedById",
                table: "Branches",
                column: "LastUpdatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
