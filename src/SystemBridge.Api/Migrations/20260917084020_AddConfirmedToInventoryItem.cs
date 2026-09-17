using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SystemBridge.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddConfirmedToInventoryItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Confirmed",
                table: "InventoryItems",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Confirmed",
                table: "InventoryItems");
        }
    }
}
