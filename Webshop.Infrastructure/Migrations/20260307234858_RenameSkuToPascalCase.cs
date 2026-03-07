using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webshop.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameSkuToPascalCase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "sku",
                table: "Products",
                newName: "Sku");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Sku",
                table: "Products",
                newName: "sku");
        }
    }
}
