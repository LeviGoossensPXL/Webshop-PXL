using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi.Migrations
{
    /// <inheritdoc />
    public partial class remove_SKU : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_StockItems_Sku",
                table: "StockItems");

            migrationBuilder.DropColumn(
                name: "Sku",
                table: "StockItems");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Sku",
                table: "StockItems",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StockItems_Sku",
                table: "StockItems",
                column: "Sku");
        }
    }
}
