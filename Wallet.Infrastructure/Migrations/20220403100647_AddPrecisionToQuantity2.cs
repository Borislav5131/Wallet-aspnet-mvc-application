using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wallet.Infrastructure.Migrations
{
    public partial class AddPrecisionToQuantity2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Quantity",
                table: "UserAssets",
                type: "decimal(12,5)",
                precision: 12,
                scale: 5,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(11,6)",
                oldPrecision: 11,
                oldScale: 6);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Quantity",
                table: "UserAssets",
                type: "decimal(11,6)",
                precision: 11,
                scale: 6,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(12,5)",
                oldPrecision: 12,
                oldScale: 5);
        }
    }
}
