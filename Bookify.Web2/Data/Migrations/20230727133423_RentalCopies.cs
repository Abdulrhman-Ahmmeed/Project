using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookify.Web2.Data.Migrations
{
    public partial class RentalCopies : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "rentals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubscriberID = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PenaltyPaid = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOnByID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rentals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_rentals_AspNetUsers_CreatedOnByID",
                        column: x => x.CreatedOnByID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_rentals_AspNetUsers_LastUpdatedById",
                        column: x => x.LastUpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_rentals_subscribers_SubscriberID",
                        column: x => x.SubscriberID,
                        principalTable: "subscribers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "rentalCopies",
                columns: table => new
                {
                    RentalId = table.Column<int>(type: "int", nullable: false),
                    BookCopyID = table.Column<int>(type: "int", nullable: false),
                    RentalDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReturnedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExtendOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rentalCopies", x => new { x.RentalId, x.BookCopyID });
                    table.ForeignKey(
                        name: "FK_rentalCopies_bookCopies_BookCopyID",
                        column: x => x.BookCopyID,
                        principalTable: "bookCopies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_rentalCopies_rentals_RentalId",
                        column: x => x.RentalId,
                        principalTable: "rentals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_rentalCopies_BookCopyID",
                table: "rentalCopies",
                column: "BookCopyID");

            migrationBuilder.CreateIndex(
                name: "IX_rentals_CreatedOnByID",
                table: "rentals",
                column: "CreatedOnByID");

            migrationBuilder.CreateIndex(
                name: "IX_rentals_LastUpdatedById",
                table: "rentals",
                column: "LastUpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_rentals_SubscriberID",
                table: "rentals",
                column: "SubscriberID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "rentalCopies");

            migrationBuilder.DropTable(
                name: "rentals");
        }
    }
}
