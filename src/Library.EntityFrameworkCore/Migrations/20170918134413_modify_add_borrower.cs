using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Library.Migrations
{
    public partial class modify_add_borrower : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "BorrowerUserId",
                table: "AppBorrowRecords",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<int>(
                name: "RenewTime",
                table: "AppBorrowRecords",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BorrowerUserId",
                table: "AppBorrowRecords");

            migrationBuilder.DropColumn(
                name: "RenewTime",
                table: "AppBorrowRecords");
        }
    }
}
