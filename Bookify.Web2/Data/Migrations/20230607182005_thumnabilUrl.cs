using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookify.Web2.Data.Migrations
{
    public partial class thumnabilUrl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageThumnabilUrl",
                table: "Books",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageThumnabilUrl",
                table: "Books");
        }
    }
}
