﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eMuhasebeServer.Infrastructure.Migrations.CompanyDb
{
    /// <inheritdoc />
    public partial class mg_15 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CashRegisterDetailOppositeId",
                table: "CashRegisterDetails",
                newName: "CashRegisterDetailId");

            migrationBuilder.RenameColumn(
                name: "BankDetailOppositeId",
                table: "BankDetails",
                newName: "CashRegisterDetailId");

            migrationBuilder.AddColumn<Guid>(
                name: "BankDetailId",
                table: "BankDetails",
                type: "uniqueidentifier",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BankDetailId",
                table: "BankDetails");

            migrationBuilder.RenameColumn(
                name: "CashRegisterDetailId",
                table: "CashRegisterDetails",
                newName: "CashRegisterDetailOppositeId");

            migrationBuilder.RenameColumn(
                name: "CashRegisterDetailId",
                table: "BankDetails",
                newName: "BankDetailOppositeId");
        }
    }
}
