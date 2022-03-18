using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BraidsAccounting.DAL.Migrations
{
    public partial class _1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Store_Items_Items_ItemId",
                table: "Store");

            migrationBuilder.DropTable(
                name: "Services_Items");

            migrationBuilder.RenameColumn(
                name: "Items_Price",
                table: "Store",
                newName: "EnumerableItem_Price");

            migrationBuilder.RenameColumn(
                name: "Items_ItemId",
                table: "Store",
                newName: "EnumerableItem_ItemId");

            migrationBuilder.RenameColumn(
                name: "Items_Count",
                table: "Store",
                newName: "EnumerableItem_Count");

            migrationBuilder.RenameIndex(
                name: "IX_Store_Items_ItemId",
                table: "Store",
                newName: "IX_Store_EnumerableItem_ItemId");

            migrationBuilder.CreateTable(
                name: "Services_WastedItems",
                columns: table => new
                {
                    ServiceId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services_WastedItems", x => new { x.ServiceId, x.Id });
                    table.ForeignKey(
                        name: "FK_Services_WastedItems_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Services_WastedItems_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Services_WastedItems_ItemId",
                table: "Services_WastedItems",
                column: "ItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Store_Items_EnumerableItem_ItemId",
                table: "Store",
                column: "EnumerableItem_ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Store_Items_EnumerableItem_ItemId",
                table: "Store");

            migrationBuilder.DropTable(
                name: "Services_WastedItems");

            migrationBuilder.RenameColumn(
                name: "EnumerableItem_Price",
                table: "Store",
                newName: "Items_Price");

            migrationBuilder.RenameColumn(
                name: "EnumerableItem_ItemId",
                table: "Store",
                newName: "Items_ItemId");

            migrationBuilder.RenameColumn(
                name: "EnumerableItem_Count",
                table: "Store",
                newName: "Items_Count");

            migrationBuilder.RenameIndex(
                name: "IX_Store_EnumerableItem_ItemId",
                table: "Store",
                newName: "IX_Store_Items_ItemId");

            migrationBuilder.CreateTable(
                name: "Services_Items",
                columns: table => new
                {
                    ServiceId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services_Items", x => new { x.ServiceId, x.Id });
                    table.ForeignKey(
                        name: "FK_Services_Items_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Services_Items_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Services_Items_ItemId",
                table: "Services_Items",
                column: "ItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Store_Items_Items_ItemId",
                table: "Store",
                column: "Items_ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
