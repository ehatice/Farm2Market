using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Farm2Market.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class imageler : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Products");

            migrationBuilder.AddColumn<byte[]>(
                name: "Image1",
                table: "Products",
                type: "longblob",
                nullable: false);

            migrationBuilder.AddColumn<byte[]>(
                name: "Image2",
                table: "Products",
                type: "longblob",
                nullable: false);

            migrationBuilder.AddColumn<byte[]>(
                name: "Image3",
                table: "Products",
                type: "longblob",
                nullable: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image1",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Image2",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Image3",
                table: "Products");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Products",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
