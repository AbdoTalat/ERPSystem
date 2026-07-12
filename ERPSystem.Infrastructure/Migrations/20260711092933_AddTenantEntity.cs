using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERPSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTenantEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
               name: "Tenants",
               columns: table => new
               {
                   Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                   Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                   IsActive = table.Column<bool>(type: "bit", nullable: false),
                   CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                   UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
               },
               constraints: table =>
               {
                   table.PrimaryKey("PK_Tenants", x => x.Id);
               });

            var defaultTenantId = new Guid("11111111-1111-1111-1111-111111111111");

            migrationBuilder.InsertData(
                table: "Tenants",
                columns: new[] { "Id", "Name", "IsActive", "CreatedAt" },
                values: new object[]
                {
                    defaultTenantId,
                    "Default Tenant",
                    true,
                    DateTime.UtcNow
                });

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Branches_DefaultBranchId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_RefreshToken_AspNetUsers_UserId",
                table: "RefreshToken");

            migrationBuilder.DropForeignKey(
                name: "FK_StockMovements_AspNetUsers_UserId",
                table: "StockMovements");

            migrationBuilder.DropIndex(
                name: "IX_StockMovements_UserId",
                table: "StockMovements");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_DefaultBranchId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "StockMovements");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "StockMovements");

            migrationBuilder.DropColumn(
                name: "DefaultBranchId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "IsDefaultBranch",
                table: "UserBranches",
                newName: "IsDefault");

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "Warehouses",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: defaultTenantId);

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "UserBranches",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: defaultTenantId);

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "Suppliers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: defaultTenantId);

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "Stocks",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: defaultTenantId);

            migrationBuilder.AddColumn<int>(
                name: "CreatedById",
                table: "StockMovements",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "StockMovements",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LastUpdatedById",
                table: "StockMovements",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdatedDate",
                table: "StockMovements",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "StockMovements",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: defaultTenantId);

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "SalesOrders",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: defaultTenantId);

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "SalesOrderLines",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: defaultTenantId);

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "PurchaseOrders",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: defaultTenantId);

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "PurchaseOrderLines",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: defaultTenantId);

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "Products",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: defaultTenantId);

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "Payments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: defaultTenantId);

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "Invoices",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: defaultTenantId);

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "GoodsReceipts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: defaultTenantId);

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "GoodsReceiptLines",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: defaultTenantId);

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "Employees",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: defaultTenantId);

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "Departments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: defaultTenantId);

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "Customers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: defaultTenantId);

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "Categories",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: defaultTenantId);

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "Branches",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: defaultTenantId);

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: defaultTenantId);

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "AspNetRoles",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: defaultTenantId);

            migrationBuilder.CreateIndex(
                name: "IX_Warehouses_TenantId",
                table: "Warehouses",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_UserBranches_TenantId",
                table: "UserBranches",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_TenantId",
                table: "Suppliers",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Stocks_TenantId",
                table: "Stocks",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_StockMovements_CreatedById",
                table: "StockMovements",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_StockMovements_LastUpdatedById",
                table: "StockMovements",
                column: "LastUpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_StockMovements_TenantId",
                table: "StockMovements",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrders_TenantId",
                table: "SalesOrders",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrderLines_TenantId",
                table: "SalesOrderLines",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_TenantId",
                table: "PurchaseOrders",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderLines_TenantId",
                table: "PurchaseOrderLines",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_TenantId",
                table: "Products",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_TenantId",
                table: "Payments",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_TenantId",
                table: "Invoices",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_GoodsReceipts_TenantId",
                table: "GoodsReceipts",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_GoodsReceiptLines_TenantId",
                table: "GoodsReceiptLines",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_TenantId",
                table: "Employees",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_TenantId",
                table: "Departments",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_TenantId",
                table: "Customers",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_TenantId",
                table: "Categories",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Branches_TenantId",
                table: "Branches",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_TenantId",
                table: "AspNetUsers",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoles_TenantId",
                table: "AspNetRoles",
                column: "TenantId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetRoles_Tenants_TenantId",
                table: "AspNetRoles",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Tenants_TenantId",
                table: "AspNetUsers",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Branches_Tenants_TenantId",
                table: "Branches",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Tenants_TenantId",
                table: "Categories",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Tenants_TenantId",
                table: "Customers",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_Tenants_TenantId",
                table: "Departments",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Tenants_TenantId",
                table: "Employees",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GoodsReceiptLines_Tenants_TenantId",
                table: "GoodsReceiptLines",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GoodsReceipts_Tenants_TenantId",
                table: "GoodsReceipts",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Tenants_TenantId",
                table: "Invoices",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Tenants_TenantId",
                table: "Payments",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Tenants_TenantId",
                table: "Products",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrderLines_Tenants_TenantId",
                table: "PurchaseOrderLines",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrders_Tenants_TenantId",
                table: "PurchaseOrders",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshToken_AspNetUsers_UserId",
                table: "RefreshToken",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SalesOrderLines_Tenants_TenantId",
                table: "SalesOrderLines",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SalesOrders_Tenants_TenantId",
                table: "SalesOrders",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StockMovements_AspNetUsers_CreatedById",
                table: "StockMovements",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StockMovements_AspNetUsers_LastUpdatedById",
                table: "StockMovements",
                column: "LastUpdatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StockMovements_Tenants_TenantId",
                table: "StockMovements",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Stocks_Tenants_TenantId",
                table: "Stocks",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Suppliers_Tenants_TenantId",
                table: "Suppliers",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserBranches_Tenants_TenantId",
                table: "UserBranches",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Warehouses_Tenants_TenantId",
                table: "Warehouses",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetRoles_Tenants_TenantId",
                table: "AspNetRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Tenants_TenantId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Branches_Tenants_TenantId",
                table: "Branches");

            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Tenants_TenantId",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Tenants_TenantId",
                table: "Customers");

            migrationBuilder.DropForeignKey(
                name: "FK_Departments_Tenants_TenantId",
                table: "Departments");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Tenants_TenantId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_GoodsReceiptLines_Tenants_TenantId",
                table: "GoodsReceiptLines");

            migrationBuilder.DropForeignKey(
                name: "FK_GoodsReceipts_Tenants_TenantId",
                table: "GoodsReceipts");

            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Tenants_TenantId",
                table: "Invoices");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Tenants_TenantId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Tenants_TenantId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrderLines_Tenants_TenantId",
                table: "PurchaseOrderLines");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrders_Tenants_TenantId",
                table: "PurchaseOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_RefreshToken_AspNetUsers_UserId",
                table: "RefreshToken");

            migrationBuilder.DropForeignKey(
                name: "FK_SalesOrderLines_Tenants_TenantId",
                table: "SalesOrderLines");

            migrationBuilder.DropForeignKey(
                name: "FK_SalesOrders_Tenants_TenantId",
                table: "SalesOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_StockMovements_AspNetUsers_CreatedById",
                table: "StockMovements");

            migrationBuilder.DropForeignKey(
                name: "FK_StockMovements_AspNetUsers_LastUpdatedById",
                table: "StockMovements");

            migrationBuilder.DropForeignKey(
                name: "FK_StockMovements_Tenants_TenantId",
                table: "StockMovements");

            migrationBuilder.DropForeignKey(
                name: "FK_Stocks_Tenants_TenantId",
                table: "Stocks");

            migrationBuilder.DropForeignKey(
                name: "FK_Suppliers_Tenants_TenantId",
                table: "Suppliers");

            migrationBuilder.DropForeignKey(
                name: "FK_UserBranches_Tenants_TenantId",
                table: "UserBranches");

            migrationBuilder.DropForeignKey(
                name: "FK_Warehouses_Tenants_TenantId",
                table: "Warehouses");

            migrationBuilder.DropTable(
                name: "Tenants");

            migrationBuilder.DropIndex(
                name: "IX_Warehouses_TenantId",
                table: "Warehouses");

            migrationBuilder.DropIndex(
                name: "IX_UserBranches_TenantId",
                table: "UserBranches");

            migrationBuilder.DropIndex(
                name: "IX_Suppliers_TenantId",
                table: "Suppliers");

            migrationBuilder.DropIndex(
                name: "IX_Stocks_TenantId",
                table: "Stocks");

            migrationBuilder.DropIndex(
                name: "IX_StockMovements_CreatedById",
                table: "StockMovements");

            migrationBuilder.DropIndex(
                name: "IX_StockMovements_LastUpdatedById",
                table: "StockMovements");

            migrationBuilder.DropIndex(
                name: "IX_StockMovements_TenantId",
                table: "StockMovements");

            migrationBuilder.DropIndex(
                name: "IX_SalesOrders_TenantId",
                table: "SalesOrders");

            migrationBuilder.DropIndex(
                name: "IX_SalesOrderLines_TenantId",
                table: "SalesOrderLines");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseOrders_TenantId",
                table: "PurchaseOrders");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseOrderLines_TenantId",
                table: "PurchaseOrderLines");

            migrationBuilder.DropIndex(
                name: "IX_Products_TenantId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Payments_TenantId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Invoices_TenantId",
                table: "Invoices");

            migrationBuilder.DropIndex(
                name: "IX_GoodsReceipts_TenantId",
                table: "GoodsReceipts");

            migrationBuilder.DropIndex(
                name: "IX_GoodsReceiptLines_TenantId",
                table: "GoodsReceiptLines");

            migrationBuilder.DropIndex(
                name: "IX_Employees_TenantId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Departments_TenantId",
                table: "Departments");

            migrationBuilder.DropIndex(
                name: "IX_Customers_TenantId",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Categories_TenantId",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Branches_TenantId",
                table: "Branches");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_TenantId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetRoles_TenantId",
                table: "AspNetRoles");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "UserBranches");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "Stocks");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "StockMovements");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "StockMovements");

            migrationBuilder.DropColumn(
                name: "LastUpdatedById",
                table: "StockMovements");

            migrationBuilder.DropColumn(
                name: "LastUpdatedDate",
                table: "StockMovements");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "StockMovements");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "SalesOrders");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "SalesOrderLines");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "PurchaseOrderLines");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "GoodsReceipts");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "GoodsReceiptLines");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "Branches");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "AspNetRoles");

            migrationBuilder.RenameColumn(
                name: "IsDefault",
                table: "UserBranches",
                newName: "IsDefaultBranch");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "StockMovements",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "StockMovements",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DefaultBranchId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StockMovements_UserId",
                table: "StockMovements",
                column: "UserId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshToken_AspNetUsers_UserId",
                table: "RefreshToken",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockMovements_AspNetUsers_UserId",
                table: "StockMovements",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
