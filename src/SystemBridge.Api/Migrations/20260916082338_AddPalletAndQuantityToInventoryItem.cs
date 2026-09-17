using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SystemBridge.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddPalletAndQuantityToInventoryItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PalletId",
                table: "InventoryItems",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "InventoryItems",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItems_PalletId",
                table: "InventoryItems",
                column: "PalletId");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryItems_Pallets_PalletId",
                table: "InventoryItems",
                column: "PalletId",
                principalTable: "Pallets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryItems_Pallets_PalletId",
                table: "InventoryItems");

            migrationBuilder.DropIndex(
                name: "IX_InventoryItems_PalletId",
                table: "InventoryItems");

            migrationBuilder.DropColumn(
                name: "PalletId",
                table: "InventoryItems");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "InventoryItems");
        }
    }
}
