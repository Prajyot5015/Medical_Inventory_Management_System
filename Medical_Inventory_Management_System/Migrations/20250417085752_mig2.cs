using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Medical_Inventory_Management_System.Migrations
{
    /// <inheritdoc />
    public partial class mig2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrderItem_Products_ProductId",
                table: "PurchaseOrderItem");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrderItem_PurchaseOrder_PurchaseOrderId",
                table: "PurchaseOrderItem");

            migrationBuilder.DropForeignKey(
                name: "FK_SalesItem_Products_ProductId",
                table: "SalesItem");

            migrationBuilder.DropForeignKey(
                name: "FK_SalesItem_Sale_SaleId",
                table: "SalesItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SalesItem",
                table: "SalesItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Sale",
                table: "Sale");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PurchaseOrderItem",
                table: "PurchaseOrderItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PurchaseOrder",
                table: "PurchaseOrder");

            migrationBuilder.RenameTable(
                name: "SalesItem",
                newName: "SalesItems");

            migrationBuilder.RenameTable(
                name: "Sale",
                newName: "Sales");

            migrationBuilder.RenameTable(
                name: "PurchaseOrderItem",
                newName: "PurchaseOrderItems");

            migrationBuilder.RenameTable(
                name: "PurchaseOrder",
                newName: "PurchaseOrders");

            migrationBuilder.RenameIndex(
                name: "IX_SalesItem_SaleId",
                table: "SalesItems",
                newName: "IX_SalesItems_SaleId");

            migrationBuilder.RenameIndex(
                name: "IX_SalesItem_ProductId",
                table: "SalesItems",
                newName: "IX_SalesItems_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_PurchaseOrderItem_PurchaseOrderId",
                table: "PurchaseOrderItems",
                newName: "IX_PurchaseOrderItems_PurchaseOrderId");

            migrationBuilder.RenameIndex(
                name: "IX_PurchaseOrderItem_ProductId",
                table: "PurchaseOrderItems",
                newName: "IX_PurchaseOrderItems_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SalesItems",
                table: "SalesItems",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Sales",
                table: "Sales",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PurchaseOrderItems",
                table: "PurchaseOrderItems",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PurchaseOrders",
                table: "PurchaseOrders",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Stocks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    CurrentStock = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stocks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stocks_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Stocks_ProductId",
                table: "Stocks",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrderItems_Products_ProductId",
                table: "PurchaseOrderItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrderItems_PurchaseOrders_PurchaseOrderId",
                table: "PurchaseOrderItems",
                column: "PurchaseOrderId",
                principalTable: "PurchaseOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SalesItems_Products_ProductId",
                table: "SalesItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SalesItems_Sales_SaleId",
                table: "SalesItems",
                column: "SaleId",
                principalTable: "Sales",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrderItems_Products_ProductId",
                table: "PurchaseOrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrderItems_PurchaseOrders_PurchaseOrderId",
                table: "PurchaseOrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_SalesItems_Products_ProductId",
                table: "SalesItems");

            migrationBuilder.DropForeignKey(
                name: "FK_SalesItems_Sales_SaleId",
                table: "SalesItems");

            migrationBuilder.DropTable(
                name: "Stocks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SalesItems",
                table: "SalesItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Sales",
                table: "Sales");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PurchaseOrders",
                table: "PurchaseOrders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PurchaseOrderItems",
                table: "PurchaseOrderItems");

            migrationBuilder.RenameTable(
                name: "SalesItems",
                newName: "SalesItem");

            migrationBuilder.RenameTable(
                name: "Sales",
                newName: "Sale");

            migrationBuilder.RenameTable(
                name: "PurchaseOrders",
                newName: "PurchaseOrder");

            migrationBuilder.RenameTable(
                name: "PurchaseOrderItems",
                newName: "PurchaseOrderItem");

            migrationBuilder.RenameIndex(
                name: "IX_SalesItems_SaleId",
                table: "SalesItem",
                newName: "IX_SalesItem_SaleId");

            migrationBuilder.RenameIndex(
                name: "IX_SalesItems_ProductId",
                table: "SalesItem",
                newName: "IX_SalesItem_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_PurchaseOrderItems_PurchaseOrderId",
                table: "PurchaseOrderItem",
                newName: "IX_PurchaseOrderItem_PurchaseOrderId");

            migrationBuilder.RenameIndex(
                name: "IX_PurchaseOrderItems_ProductId",
                table: "PurchaseOrderItem",
                newName: "IX_PurchaseOrderItem_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SalesItem",
                table: "SalesItem",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Sale",
                table: "Sale",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PurchaseOrder",
                table: "PurchaseOrder",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PurchaseOrderItem",
                table: "PurchaseOrderItem",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrderItem_Products_ProductId",
                table: "PurchaseOrderItem",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrderItem_PurchaseOrder_PurchaseOrderId",
                table: "PurchaseOrderItem",
                column: "PurchaseOrderId",
                principalTable: "PurchaseOrder",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SalesItem_Products_ProductId",
                table: "SalesItem",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SalesItem_Sale_SaleId",
                table: "SalesItem",
                column: "SaleId",
                principalTable: "Sale",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
