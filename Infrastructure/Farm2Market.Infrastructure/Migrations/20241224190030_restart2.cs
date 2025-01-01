using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Farm2Market.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class restart2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TempColumn",
                table: "Categories");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TempColumn",
                table: "Categories",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
