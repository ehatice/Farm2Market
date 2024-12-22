using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Farm2Market.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FavoriDuzeltmesi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MarketFavorites_MarketReceivers_MarketReceiverId",
                table: "MarketFavorites");

            migrationBuilder.DropIndex(
                name: "IX_MarketFavorites_MarketReceiverId",
                table: "MarketFavorites");

            migrationBuilder.AlterColumn<Guid>(
                name: "MarketReceiverId",
                table: "MarketFavorites",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "MarketReceiverId1",
                table: "MarketFavorites",
                type: "varchar(255)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_MarketFavorites_MarketReceiverId1",
                table: "MarketFavorites",
                column: "MarketReceiverId1");

            migrationBuilder.AddForeignKey(
                name: "FK_MarketFavorites_MarketReceivers_MarketReceiverId1",
                table: "MarketFavorites",
                column: "MarketReceiverId1",
                principalTable: "MarketReceivers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MarketFavorites_MarketReceivers_MarketReceiverId1",
                table: "MarketFavorites");

            migrationBuilder.DropIndex(
                name: "IX_MarketFavorites_MarketReceiverId1",
                table: "MarketFavorites");

            migrationBuilder.DropColumn(
                name: "MarketReceiverId1",
                table: "MarketFavorites");

            migrationBuilder.AlterColumn<string>(
                name: "MarketReceiverId",
                table: "MarketFavorites",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_MarketFavorites_MarketReceiverId",
                table: "MarketFavorites",
                column: "MarketReceiverId");

            migrationBuilder.AddForeignKey(
                name: "FK_MarketFavorites_MarketReceivers_MarketReceiverId",
                table: "MarketFavorites",
                column: "MarketReceiverId",
                principalTable: "MarketReceivers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
