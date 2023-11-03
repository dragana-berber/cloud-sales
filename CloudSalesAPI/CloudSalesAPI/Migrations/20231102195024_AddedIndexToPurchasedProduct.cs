using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CloudSalesAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddedIndexToPurchasedProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchasedProducts_CustomerAccounts_CustomerAccountId",
                table: "PurchasedProducts");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "PurchasedProducts");

            migrationBuilder.AlterColumn<int>(
                name: "CustomerAccountId",
                table: "PurchasedProducts",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PurchasedProducts_ExternalId_CustomerAccountId",
                table: "PurchasedProducts",
                columns: new[] { "ExternalId", "CustomerAccountId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchasedProducts_CustomerAccounts_CustomerAccountId",
                table: "PurchasedProducts",
                column: "CustomerAccountId",
                principalTable: "CustomerAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchasedProducts_CustomerAccounts_CustomerAccountId",
                table: "PurchasedProducts");

            migrationBuilder.DropIndex(
                name: "IX_PurchasedProducts_ExternalId_CustomerAccountId",
                table: "PurchasedProducts");

            migrationBuilder.AlterColumn<int>(
                name: "CustomerAccountId",
                table: "PurchasedProducts",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "AccountId",
                table: "PurchasedProducts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchasedProducts_CustomerAccounts_CustomerAccountId",
                table: "PurchasedProducts",
                column: "CustomerAccountId",
                principalTable: "CustomerAccounts",
                principalColumn: "Id");
        }
    }
}
