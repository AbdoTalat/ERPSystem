using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERPSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AlterStockAddAvailableQuantity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastStockUpdate",
                table: "Stocks");

            migrationBuilder.AddColumn<int>(
                name: "AvailableQuantity",
                table: "Stocks",
                type: "int",
                nullable: false,
                computedColumnSql: "[Quantity] - ([ReservedQuantity] + [DamagedQuantity])",
                stored: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvailableQuantity",
                table: "Stocks");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastStockUpdate",
                table: "Stocks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
