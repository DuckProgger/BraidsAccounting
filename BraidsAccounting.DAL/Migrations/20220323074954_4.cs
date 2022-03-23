using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BraidsAccounting.DAL.Migrations
{
    public partial class _4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_ItemPrices_ManufacturerId",
                table: "Items");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ItemPrices",
                table: "ItemPrices");

            migrationBuilder.RenameTable(
                name: "ItemPrices",
                newName: "Manufacturers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Manufacturers",
                table: "Manufacturers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Manufacturers_ManufacturerId",
                table: "Items",
                column: "ManufacturerId",
                principalTable: "Manufacturers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Manufacturers_ManufacturerId",
                table: "Items");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Manufacturers",
                table: "Manufacturers");

            migrationBuilder.RenameTable(
                name: "Manufacturers",
                newName: "ItemPrices");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ItemPrices",
                table: "ItemPrices",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_ItemPrices_ManufacturerId",
                table: "Items",
                column: "ManufacturerId",
                principalTable: "ItemPrices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
