using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LMS.Api.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuthorWorks",
                columns: table => new
                {
                    WorkId = table.Column<int>(type: "int", nullable: false),
                    AuthorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorWorks", x => new { x.AuthorId, x.WorkId });
                });

            migrationBuilder.CreateTable(
                name: "Genres",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genres", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Types",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Types", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Authors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AuthorWorkAuthorId = table.Column<int>(type: "int", nullable: true),
                    AuthorWorkWorkId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Authors_AuthorWorks_AuthorWorkAuthorId_AuthorWorkWorkId",
                        columns: x => new { x.AuthorWorkAuthorId, x.AuthorWorkWorkId },
                        principalTable: "AuthorWorks",
                        principalColumns: new[] { "AuthorId", "WorkId" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Works",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GenreId = table.Column<int>(type: "int", nullable: false),
                    TypeId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Level = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PublicationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AuthorWorkAuthorId = table.Column<int>(type: "int", nullable: true),
                    AuthorWorkWorkId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Works", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Works_AuthorWorks_AuthorWorkAuthorId_AuthorWorkWorkId",
                        columns: x => new { x.AuthorWorkAuthorId, x.AuthorWorkWorkId },
                        principalTable: "AuthorWorks",
                        principalColumns: new[] { "AuthorId", "WorkId" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Works_Genres_GenreId",
                        column: x => x.GenreId,
                        principalTable: "Genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Works_Types_TypeId",
                        column: x => x.TypeId,
                        principalTable: "Types",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AuthorWork",
                columns: table => new
                {
                    AuthorsId = table.Column<int>(type: "int", nullable: false),
                    WorksId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorWork", x => new { x.AuthorsId, x.WorksId });
                    table.ForeignKey(
                        name: "FK_AuthorWork_Authors_AuthorsId",
                        column: x => x.AuthorsId,
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuthorWork_Works_WorksId",
                        column: x => x.WorksId,
                        principalTable: "Works",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Authors_AuthorWorkAuthorId_AuthorWorkWorkId",
                table: "Authors",
                columns: new[] { "AuthorWorkAuthorId", "AuthorWorkWorkId" });

            migrationBuilder.CreateIndex(
                name: "IX_AuthorWork_WorksId",
                table: "AuthorWork",
                column: "WorksId");

            migrationBuilder.CreateIndex(
                name: "IX_Works_AuthorWorkAuthorId_AuthorWorkWorkId",
                table: "Works",
                columns: new[] { "AuthorWorkAuthorId", "AuthorWorkWorkId" });

            migrationBuilder.CreateIndex(
                name: "IX_Works_GenreId",
                table: "Works",
                column: "GenreId");

            migrationBuilder.CreateIndex(
                name: "IX_Works_TypeId",
                table: "Works",
                column: "TypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthorWork");

            migrationBuilder.DropTable(
                name: "Authors");

            migrationBuilder.DropTable(
                name: "Works");

            migrationBuilder.DropTable(
                name: "AuthorWorks");

            migrationBuilder.DropTable(
                name: "Genres");

            migrationBuilder.DropTable(
                name: "Types");
        }
    }
}
