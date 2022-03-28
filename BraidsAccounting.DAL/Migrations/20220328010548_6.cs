using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BraidsAccounting.DAL.Migrations
{
    public partial class _6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Services",
                newName: "WorkerName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WorkerName",
                table: "Services",
                newName: "Name");
        }
    }
}
