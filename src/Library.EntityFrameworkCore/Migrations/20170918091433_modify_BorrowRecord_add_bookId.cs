using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Library.Migrations
{
    public partial class modify_BorrowRecord_add_bookId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppBorrowRecords_AppBooks_BookId",
                table: "AppBorrowRecords");

            migrationBuilder.AlterColumn<long>(
                name: "BookId",
                table: "AppBorrowRecords",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AppBorrowRecords_AppBooks_BookId",
                table: "AppBorrowRecords",
                column: "BookId",
                principalTable: "AppBooks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppBorrowRecords_AppBooks_BookId",
                table: "AppBorrowRecords");

            migrationBuilder.AlterColumn<long>(
                name: "BookId",
                table: "AppBorrowRecords",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_AppBorrowRecords_AppBooks_BookId",
                table: "AppBorrowRecords",
                column: "BookId",
                principalTable: "AppBooks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
