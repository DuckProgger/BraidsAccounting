using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BraidsAccounting.DAL.Migrations
{
    public partial class _3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_ItemPrices_ItemPriceId",
                table: "Items");

            migrationBuilder.RenameColumn(
                name: "ItemPriceId",
                table: "Items",
                newName: "ManufacturerId");

            migrationBuilder.RenameIndex(
                name: "IX_Items_ItemPriceId",
                table: "Items",
                newName: "IX_Items_ManufacturerId");

            migrationBuilder.RenameColumn(
                name: "Manufacturer",
                table: "ItemPrices",
                newName: "Name");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_ItemPrices_ManufacturerId",
                table: "Items",
                column: "ManufacturerId",
                principalTable: "ItemPrices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_ItemPrices_ManufacturerId",
                table: "Items");

            migrationBuilder.RenameColumn(
                name: "ManufacturerId",
                table: "Items",
                newName: "ItemPriceId");

            migrationBuilder.RenameIndex(
                name: "IX_Items_ManufacturerId",
                table: "Items",
                newName: "IX_Items_ItemPriceId");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "ItemPrices",
                newName: "Manufacturer");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_ItemPrices_ItemPriceId",
                table: "Items",
                column: "ItemPriceId",
                principalTable: "ItemPrices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
