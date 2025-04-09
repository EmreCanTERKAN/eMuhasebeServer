using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eMuhasebeServer.Infrastructure.Migrations.CompanyDb
{
    /// <inheritdoc />
    public partial class mg_7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CashRegisterDetailId",
                table: "CashRegisterDetails",
                newName: "CashRegisterDetailOppositeId");

            migrationBuilder.RenameColumn(
                name: "BankDetailId",
                table: "BankDetails",
                newName: "BankDetailOppositeId");

            migrationBuilder.CreateIndex(
                name: "IX_CashRegisterDetails_CashRegisterDetailOppositeId",
                table: "CashRegisterDetails",
                column: "CashRegisterDetailOppositeId");

            migrationBuilder.CreateIndex(
                name: "IX_BankDetails_BankDetailOppositeId",
                table: "BankDetails",
                column: "BankDetailOppositeId");

            migrationBuilder.AddForeignKey(
                name: "FK_BankDetails_BankDetails_BankDetailOppositeId",
                table: "BankDetails",
                column: "BankDetailOppositeId",
                principalTable: "BankDetails",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CashRegisterDetails_CashRegisterDetails_CashRegisterDetailOppositeId",
                table: "CashRegisterDetails",
                column: "CashRegisterDetailOppositeId",
                principalTable: "CashRegisterDetails",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankDetails_BankDetails_BankDetailOppositeId",
                table: "BankDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_CashRegisterDetails_CashRegisterDetails_CashRegisterDetailOppositeId",
                table: "CashRegisterDetails");

            migrationBuilder.DropIndex(
                name: "IX_CashRegisterDetails_CashRegisterDetailOppositeId",
                table: "CashRegisterDetails");

            migrationBuilder.DropIndex(
                name: "IX_BankDetails_BankDetailOppositeId",
                table: "BankDetails");

            migrationBuilder.RenameColumn(
                name: "CashRegisterDetailOppositeId",
                table: "CashRegisterDetails",
                newName: "CashRegisterDetailId");

            migrationBuilder.RenameColumn(
                name: "BankDetailOppositeId",
                table: "BankDetails",
                newName: "BankDetailId");
        }
    }
}
