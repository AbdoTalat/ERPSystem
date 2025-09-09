using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERPSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AlterSalesOrderLine : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WarehouseId",
                table: "SalesOrderLines",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrderLines_WarehouseId",
                table: "SalesOrderLines",
                column: "WarehouseId");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesOrderLines_Warehouses_WarehouseId",
                table: "SalesOrderLines",
                column: "WarehouseId",
                principalTable: "Warehouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SalesOrderLines_Warehouses_WarehouseId",
                table: "SalesOrderLines");

            migrationBuilder.DropIndex(
                name: "IX_SalesOrderLines_WarehouseId",
                table: "SalesOrderLines");

            migrationBuilder.DropColumn(
                name: "WarehouseId",
                table: "SalesOrderLines");
        }
    }
}
