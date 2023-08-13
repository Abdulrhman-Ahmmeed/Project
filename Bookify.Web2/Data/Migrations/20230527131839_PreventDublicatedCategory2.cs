using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookify.Web2.Data.Migrations
{
    public partial class PreventDublicatedCategory2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_categories_Name",
                table: "categories");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_categories_Name",
                table: "categories",
                column: "Name",
                unique: true);
        }
    }
}
