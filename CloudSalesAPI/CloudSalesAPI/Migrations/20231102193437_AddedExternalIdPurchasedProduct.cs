using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CloudSalesAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddedExternalIdPurchasedProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ExternalId",
                table: "PurchasedProducts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExternalId",
                table: "PurchasedProducts");
        }
    }
}
