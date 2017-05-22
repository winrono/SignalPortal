using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SignalmanPortal.Migrations
{
    public partial class books_optional_category : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_CategoriesOfBooks_CategoryId",
                table: "Books");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "Books",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Books_CategoriesOfBooks_CategoryId",
                table: "Books",
                column: "CategoryId",
                principalTable: "CategoriesOfBooks",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_CategoriesOfBooks_CategoryId",
                table: "Books");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "Books",
                nullable: false);

            migrationBuilder.AddForeignKey(
                name: "FK_Books_CategoriesOfBooks_CategoryId",
                table: "Books",
                column: "CategoryId",
                principalTable: "CategoriesOfBooks",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
