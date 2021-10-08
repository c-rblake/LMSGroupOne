using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LMSGroupOne.Migrations
{
    public partial class StringPersonId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documents_AspNetUsers_PersonId1",
                table: "Documents");

            migrationBuilder.DropIndex(
                name: "IX_Documents_PersonId1",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "PersonId1",
                table: "Documents");

            migrationBuilder.AlterColumn<string>(
                name: "PersonId",
                table: "Documents",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_PersonId",
                table: "Documents",
                column: "PersonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_AspNetUsers_PersonId",
                table: "Documents",
                column: "PersonId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documents_AspNetUsers_PersonId",
                table: "Documents");

            migrationBuilder.DropIndex(
                name: "IX_Documents_PersonId",
                table: "Documents");

            migrationBuilder.AlterColumn<Guid>(
                name: "PersonId",
                table: "Documents",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "PersonId1",
                table: "Documents",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Documents_PersonId1",
                table: "Documents",
                column: "PersonId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_AspNetUsers_PersonId1",
                table: "Documents",
                column: "PersonId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
