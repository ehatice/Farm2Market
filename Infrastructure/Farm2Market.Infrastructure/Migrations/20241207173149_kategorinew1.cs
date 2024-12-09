using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Farm2Market.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class kategorinew1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.CreateTable(
	name: "ProductImages",
	columns: table => new
	{
		Id = table.Column<int>(nullable: false)
			.Annotation("SqlServer:Identity", "1, 1"),
		ProductId = table.Column<int>(nullable: false),
		Image = table.Column<byte[]>(nullable: false)
	},
	constraints: table =>
	{
		table.PrimaryKey("PK_ProductImages", x => x.Id);
		table.ForeignKey(
			name: "FK_ProductImages_Products_ProductId",
			column: x => x.ProductId,
			principalTable: "Products",
			principalColumn: "Id",
			onDelete: ReferentialAction.Cascade);
	});

		}

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

			migrationBuilder.CreateTable(
				name: "ProductImages",
				columns: table => new
				{
					Id = table.Column<int>(nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					ProductId = table.Column<int>(nullable: false),
					Image = table.Column<byte[]>(nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_ProductImages", x => x.Id);
					table.ForeignKey(
						name: "FK_ProductImages_Products_ProductId",
						column: x => x.ProductId,
						principalTable: "Products",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});



		}
    }
}
