using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace iBay.Migrations
{
    /// <inheritdoc />
    public partial class Update17 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductCart_Product_productId",
                table: "ProductCart");

            migrationBuilder.DropIndex(
                name: "IX_ProductCart_productId",
                table: "ProductCart");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ProductCart_productId",
                table: "ProductCart",
                column: "productId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCart_Product_productId",
                table: "ProductCart",
                column: "productId",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
