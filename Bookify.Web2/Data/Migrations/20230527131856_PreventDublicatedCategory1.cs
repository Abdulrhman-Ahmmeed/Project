using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookify.Web2.Data.Migrations
{
    public partial class PreventDublicatedCategory1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_categories_Name",
                table: "categories",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_categories_Name",
                table: "categories");
        }
    }
}
