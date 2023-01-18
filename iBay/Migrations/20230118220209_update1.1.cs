using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace iBay.Migrations
{
    /// <inheritdoc />
    public partial class update11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cart_User_userId",
                table: "Cart");

            migrationBuilder.RenameColumn(
                name: "userId",
                table: "Cart",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "isValidated",
                table: "Cart",
                newName: "IsValidated");

            migrationBuilder.RenameColumn(
                name: "dateValidation",
                table: "Cart",
                newName: "DateValidation");

            migrationBuilder.RenameIndex(
                name: "IX_Cart_userId",
                table: "Cart",
                newName: "IX_Cart_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cart_User_UserId",
                table: "Cart",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cart_User_UserId",
                table: "Cart");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Cart",
                newName: "userId");

            migrationBuilder.RenameColumn(
                name: "IsValidated",
                table: "Cart",
                newName: "isValidated");

            migrationBuilder.RenameColumn(
                name: "DateValidation",
                table: "Cart",
                newName: "dateValidation");

            migrationBuilder.RenameIndex(
                name: "IX_Cart_UserId",
                table: "Cart",
                newName: "IX_Cart_userId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cart_User_userId",
                table: "Cart",
                column: "userId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
