using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Farm2Market.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CartBaglantii : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MarketReceiverId",
                table: "Carts",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_MarketReceiverId",
                table: "Carts",
                column: "MarketReceiverId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_MarketReceivers_MarketReceiverId",
                table: "Carts",
                column: "MarketReceiverId",
                principalTable: "MarketReceivers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carts_MarketReceivers_MarketReceiverId",
                table: "Carts");

            migrationBuilder.DropIndex(
                name: "IX_Carts_MarketReceiverId",
                table: "Carts");

            migrationBuilder.DropColumn(
                name: "MarketReceiverId",
                table: "Carts");
        }
    }
}
