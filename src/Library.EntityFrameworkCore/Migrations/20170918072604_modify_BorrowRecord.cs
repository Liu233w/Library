using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Library.Migrations
{
    public partial class modify_BorrowRecord : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_AppBorrowRecords_CreatorUserId",
                table: "AppBorrowRecords",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AppBorrowRecords_DeleterUserId",
                table: "AppBorrowRecords",
                column: "DeleterUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppBorrowRecords_AbpUsers_CreatorUserId",
                table: "AppBorrowRecords",
                column: "CreatorUserId",
                principalTable: "AbpUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AppBorrowRecords_AbpUsers_DeleterUserId",
                table: "AppBorrowRecords",
                column: "DeleterUserId",
                principalTable: "AbpUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppBorrowRecords_AbpUsers_CreatorUserId",
                table: "AppBorrowRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_AppBorrowRecords_AbpUsers_DeleterUserId",
                table: "AppBorrowRecords");

            migrationBuilder.DropIndex(
                name: "IX_AppBorrowRecords_CreatorUserId",
                table: "AppBorrowRecords");

            migrationBuilder.DropIndex(
                name: "IX_AppBorrowRecords_DeleterUserId",
                table: "AppBorrowRecords");
        }
    }
}
