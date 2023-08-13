using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookify.Web2.Data.Migrations
{
    public partial class publicImageIdFOrDeletingCloudinary : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "publicImageId",
                table: "Books",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "publicImageId",
                table: "Books");
        }
    }
}
