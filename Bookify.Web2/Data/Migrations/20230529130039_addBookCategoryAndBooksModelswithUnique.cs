using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookify.Web2.Data.Migrations
{
    public partial class addBookCategoryAndBooksModelswithUnique : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Books_Title_AuthorId",
                table: "Books",
                columns: new[] { "Title", "AuthorId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Books_Title_AuthorId",
                table: "Books");
        }
    }
}
