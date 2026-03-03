using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webshop.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addappuser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Adressen_DeliveryAddressAddressId",
                table: "Orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Adressen",
                table: "Adressen");

            migrationBuilder.RenameTable(
                name: "Adressen",
                newName: "Addresses");

            migrationBuilder.AddColumn<int>(
                name: "AppUserId",
                table: "AspNetUsers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "AspNetUsers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Addresses",
                table: "Addresses",
                column: "AddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Addresses_DeliveryAddressAddressId",
                table: "Orders",
                column: "DeliveryAddressAddressId",
                principalTable: "Addresses",
                principalColumn: "AddressId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Addresses_DeliveryAddressAddressId",
                table: "Orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Addresses",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "AspNetUsers");

            migrationBuilder.RenameTable(
                name: "Addresses",
                newName: "Adressen");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Adressen",
                table: "Adressen",
                column: "AddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Adressen_DeliveryAddressAddressId",
                table: "Orders",
                column: "DeliveryAddressAddressId",
                principalTable: "Adressen",
                principalColumn: "AddressId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
