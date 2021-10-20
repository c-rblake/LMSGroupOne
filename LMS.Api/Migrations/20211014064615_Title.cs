using Microsoft.EntityFrameworkCore.Migrations;

namespace LMS.Api.Migrations
{
    public partial class Title : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Works",
                newName: "Title");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Works",
                newName: "Name");
        }
    }
}
