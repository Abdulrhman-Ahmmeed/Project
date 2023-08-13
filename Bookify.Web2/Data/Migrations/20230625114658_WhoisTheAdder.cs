using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookify.Web2.Data.Migrations
{
    public partial class WhoisTheAdder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "LastUpdated",
                table: "categories",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "CreatedOnByID",
                table: "categories",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastUpdatedById",
                table: "categories",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastUpdated",
                table: "Books",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "CreatedOnByID",
                table: "Books",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastUpdatedById",
                table: "Books",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastUpdated",
                table: "bookCopies",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "CreatedOnByID",
                table: "bookCopies",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastUpdatedById",
                table: "bookCopies",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastUpdated",
                table: "authors",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "CreatedOnByID",
                table: "authors",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastUpdatedById",
                table: "authors",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedOnByID",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastUpdatedById",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_categories_CreatedOnByID",
                table: "categories",
                column: "CreatedOnByID");

            migrationBuilder.CreateIndex(
                name: "IX_categories_LastUpdatedById",
                table: "categories",
                column: "LastUpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Books_CreatedOnByID",
                table: "Books",
                column: "CreatedOnByID");

            migrationBuilder.CreateIndex(
                name: "IX_Books_LastUpdatedById",
                table: "Books",
                column: "LastUpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_bookCopies_CreatedOnByID",
                table: "bookCopies",
                column: "CreatedOnByID");

            migrationBuilder.CreateIndex(
                name: "IX_bookCopies_LastUpdatedById",
                table: "bookCopies",
                column: "LastUpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_authors_CreatedOnByID",
                table: "authors",
                column: "CreatedOnByID");

            migrationBuilder.CreateIndex(
                name: "IX_authors_LastUpdatedById",
                table: "authors",
                column: "LastUpdatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_authors_AspNetUsers_CreatedOnByID",
                table: "authors",
                column: "CreatedOnByID",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_authors_AspNetUsers_LastUpdatedById",
                table: "authors",
                column: "LastUpdatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_bookCopies_AspNetUsers_CreatedOnByID",
                table: "bookCopies",
                column: "CreatedOnByID",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_bookCopies_AspNetUsers_LastUpdatedById",
                table: "bookCopies",
                column: "LastUpdatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_AspNetUsers_CreatedOnByID",
                table: "Books",
                column: "CreatedOnByID",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_AspNetUsers_LastUpdatedById",
                table: "Books",
                column: "LastUpdatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_categories_AspNetUsers_CreatedOnByID",
                table: "categories",
                column: "CreatedOnByID",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_categories_AspNetUsers_LastUpdatedById",
                table: "categories",
                column: "LastUpdatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_authors_AspNetUsers_CreatedOnByID",
                table: "authors");

            migrationBuilder.DropForeignKey(
                name: "FK_authors_AspNetUsers_LastUpdatedById",
                table: "authors");

            migrationBuilder.DropForeignKey(
                name: "FK_bookCopies_AspNetUsers_CreatedOnByID",
                table: "bookCopies");

            migrationBuilder.DropForeignKey(
                name: "FK_bookCopies_AspNetUsers_LastUpdatedById",
                table: "bookCopies");

            migrationBuilder.DropForeignKey(
                name: "FK_Books_AspNetUsers_CreatedOnByID",
                table: "Books");

            migrationBuilder.DropForeignKey(
                name: "FK_Books_AspNetUsers_LastUpdatedById",
                table: "Books");

            migrationBuilder.DropForeignKey(
                name: "FK_categories_AspNetUsers_CreatedOnByID",
                table: "categories");

            migrationBuilder.DropForeignKey(
                name: "FK_categories_AspNetUsers_LastUpdatedById",
                table: "categories");

            migrationBuilder.DropIndex(
                name: "IX_categories_CreatedOnByID",
                table: "categories");

            migrationBuilder.DropIndex(
                name: "IX_categories_LastUpdatedById",
                table: "categories");

            migrationBuilder.DropIndex(
                name: "IX_Books_CreatedOnByID",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_LastUpdatedById",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_bookCopies_CreatedOnByID",
                table: "bookCopies");

            migrationBuilder.DropIndex(
                name: "IX_bookCopies_LastUpdatedById",
                table: "bookCopies");

            migrationBuilder.DropIndex(
                name: "IX_authors_CreatedOnByID",
                table: "authors");

            migrationBuilder.DropIndex(
                name: "IX_authors_LastUpdatedById",
                table: "authors");

            migrationBuilder.DropColumn(
                name: "CreatedOnByID",
                table: "categories");

            migrationBuilder.DropColumn(
                name: "LastUpdatedById",
                table: "categories");

            migrationBuilder.DropColumn(
                name: "CreatedOnByID",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "LastUpdatedById",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "CreatedOnByID",
                table: "bookCopies");

            migrationBuilder.DropColumn(
                name: "LastUpdatedById",
                table: "bookCopies");

            migrationBuilder.DropColumn(
                name: "CreatedOnByID",
                table: "authors");

            migrationBuilder.DropColumn(
                name: "LastUpdatedById",
                table: "authors");

            migrationBuilder.DropColumn(
                name: "CreatedOnByID",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastUpdatedById",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastUpdated",
                table: "categories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastUpdated",
                table: "Books",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastUpdated",
                table: "bookCopies",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastUpdated",
                table: "authors",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }
    }
}
