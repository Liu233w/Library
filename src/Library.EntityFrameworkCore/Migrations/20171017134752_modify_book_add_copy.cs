using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Library.Migrations
{
    public partial class modify_book_add_copy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppBorrowRecords_AppBooks_BookId",
                table: "AppBorrowRecords");

            migrationBuilder.DropIndex(
                name: "IX_AppBorrowRecords_BookId",
                table: "AppBorrowRecords");

            migrationBuilder.DropColumn(
                name: "BookId",
                table: "AppBorrowRecords");

            migrationBuilder.DropColumn(
                name: "Count",
                table: "AppBooks");

            migrationBuilder.AddColumn<long>(
                name: "CopyId",
                table: "AppBorrowRecords",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "AppBooks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AppBookCopys",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BookId = table.Column<long>(type: "bigint", nullable: false),
                    BorrowRecordId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppBookCopys", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppBookCopys_AppBooks_BookId",
                        column: x => x.BookId,
                        principalTable: "AppBooks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppBookCopys_AppBorrowRecords_BorrowRecordId",
                        column: x => x.BorrowRecordId,
                        principalTable: "AppBorrowRecords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppBookCopys_BookId",
                table: "AppBookCopys",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_AppBookCopys_BorrowRecordId",
                table: "AppBookCopys",
                column: "BorrowRecordId",
                unique: true,
                filter: "[BorrowRecordId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppBookCopys");

            migrationBuilder.DropColumn(
                name: "CopyId",
                table: "AppBorrowRecords");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "AppBooks");

            migrationBuilder.AddColumn<long>(
                name: "BookId",
                table: "AppBorrowRecords",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<int>(
                name: "Count",
                table: "AppBooks",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AppBorrowRecords_BookId",
                table: "AppBorrowRecords",
                column: "BookId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppBorrowRecords_AppBooks_BookId",
                table: "AppBorrowRecords",
                column: "BookId",
                principalTable: "AppBooks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
