using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookify.Web2.Data.Migrations
{
    public partial class Subscriberwithrestricts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookCategories_Books_BookId",
                table: "BookCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_BookCategories_categories_CategoryId",
                table: "BookCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_bookCopies_Books_BookId",
                table: "bookCopies");

            migrationBuilder.DropForeignKey(
                name: "FK_Books_authors_AuthorId",
                table: "Books");

            migrationBuilder.CreateTable(
                name: "governorates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOnByID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_governorates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_governorates_AspNetUsers_CreatedOnByID",
                        column: x => x.CreatedOnByID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_governorates_AspNetUsers_LastUpdatedById",
                        column: x => x.LastUpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "areas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    GovernorateId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOnByID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_areas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_areas_AspNetUsers_CreatedOnByID",
                        column: x => x.CreatedOnByID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_areas_AspNetUsers_LastUpdatedById",
                        column: x => x.LastUpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_areas_governorates_GovernorateId",
                        column: x => x.GovernorateId,
                        principalTable: "governorates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "subscribers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NationalId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    MobileNumber = table.Column<string>(type: "nvarchar(14)", maxLength: 14, nullable: false),
                    HasWhatsApp = table.Column<bool>(type: "bit", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ImageThumnabilUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    GovernorateId = table.Column<int>(type: "int", nullable: false),
                    areaId = table.Column<int>(type: "int", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsBlackedList = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOnByID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_subscribers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_subscribers_areas_areaId",
                        column: x => x.areaId,
                        principalTable: "areas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_subscribers_AspNetUsers_CreatedOnByID",
                        column: x => x.CreatedOnByID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_subscribers_AspNetUsers_LastUpdatedById",
                        column: x => x.LastUpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_subscribers_governorates_GovernorateId",
                        column: x => x.GovernorateId,
                        principalTable: "governorates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_areas_CreatedOnByID",
                table: "areas",
                column: "CreatedOnByID");

            migrationBuilder.CreateIndex(
                name: "IX_areas_GovernorateId",
                table: "areas",
                column: "GovernorateId");

            migrationBuilder.CreateIndex(
                name: "IX_areas_LastUpdatedById",
                table: "areas",
                column: "LastUpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_areas_Name_GovernorateId",
                table: "areas",
                columns: new[] { "Name", "GovernorateId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_governorates_CreatedOnByID",
                table: "governorates",
                column: "CreatedOnByID");

            migrationBuilder.CreateIndex(
                name: "IX_governorates_LastUpdatedById",
                table: "governorates",
                column: "LastUpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_governorates_Name",
                table: "governorates",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_subscribers_areaId",
                table: "subscribers",
                column: "areaId");

            migrationBuilder.CreateIndex(
                name: "IX_subscribers_CreatedOnByID",
                table: "subscribers",
                column: "CreatedOnByID");

            migrationBuilder.CreateIndex(
                name: "IX_subscribers_Email",
                table: "subscribers",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_subscribers_GovernorateId",
                table: "subscribers",
                column: "GovernorateId");

            migrationBuilder.CreateIndex(
                name: "IX_subscribers_LastUpdatedById",
                table: "subscribers",
                column: "LastUpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_subscribers_MobileNumber",
                table: "subscribers",
                column: "MobileNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_subscribers_NationalId",
                table: "subscribers",
                column: "NationalId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BookCategories_Books_BookId",
                table: "BookCategories",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BookCategories_categories_CategoryId",
                table: "BookCategories",
                column: "CategoryId",
                principalTable: "categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_bookCopies_Books_BookId",
                table: "bookCopies",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Books_authors_AuthorId",
                table: "Books",
                column: "AuthorId",
                principalTable: "authors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookCategories_Books_BookId",
                table: "BookCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_BookCategories_categories_CategoryId",
                table: "BookCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_bookCopies_Books_BookId",
                table: "bookCopies");

            migrationBuilder.DropForeignKey(
                name: "FK_Books_authors_AuthorId",
                table: "Books");

            migrationBuilder.DropTable(
                name: "subscribers");

            migrationBuilder.DropTable(
                name: "areas");

            migrationBuilder.DropTable(
                name: "governorates");

            migrationBuilder.AddForeignKey(
                name: "FK_BookCategories_Books_BookId",
                table: "BookCategories",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookCategories_categories_CategoryId",
                table: "BookCategories",
                column: "CategoryId",
                principalTable: "categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_bookCopies_Books_BookId",
                table: "bookCopies",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Books_authors_AuthorId",
                table: "Books",
                column: "AuthorId",
                principalTable: "authors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
