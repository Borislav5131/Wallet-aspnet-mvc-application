using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wallet.Infrastructure.Migrations
{
    public partial class CategoriesSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[] { new Guid("17cb034e-ebbd-4f36-9e35-4fc2f134ff45"), "A stock is a security that represents the ownership of a fraction of a corporation.", "Stock" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[] { new Guid("cfeb0745-c0a8-4e84-b8bd-b32b4dfc9374"), "Crypto currency, sometimes called crypto-currency or crypto, is any form of currency that exists digitally or virtually and uses cryptography to secure transactions.", "Crypto" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[] { new Guid("f1be908e-0091-42bf-8312-1548c71a1548"), "Currency is a medium of exchange for goods and services.", "Currency" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("17cb034e-ebbd-4f36-9e35-4fc2f134ff45"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("cfeb0745-c0a8-4e84-b8bd-b32b4dfc9374"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("f1be908e-0091-42bf-8312-1548c71a1548"));
        }
    }
}
