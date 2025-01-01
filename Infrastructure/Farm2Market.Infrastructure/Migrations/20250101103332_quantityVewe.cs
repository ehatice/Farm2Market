using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Farm2Market.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class quantityVewe : Migration
    {
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.Sql("ALTER TABLE `CartItems` CHANGE `Quantity` `WeightOrAmount` INT NOT NULL;");
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.Sql("ALTER TABLE `CartItems` CHANGE `WeightOrAmount` `Quantity` INT NOT NULL;");
		}
	}
}
