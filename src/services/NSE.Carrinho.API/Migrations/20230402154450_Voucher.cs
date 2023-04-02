using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NSE.Carrinho.API.Migrations
{
    /// <inheritdoc />
    public partial class Voucher : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "VoucherCodigo",
                table: "CustomerCart",
                newName: "VoucherCode");

            migrationBuilder.RenameColumn(
                name: "Percentual",
                table: "CustomerCart",
                newName: "Percentage");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "VoucherCode",
                table: "CustomerCart",
                newName: "VoucherCodigo");

            migrationBuilder.RenameColumn(
                name: "Percentage",
                table: "CustomerCart",
                newName: "Percentual");
        }
    }
}
