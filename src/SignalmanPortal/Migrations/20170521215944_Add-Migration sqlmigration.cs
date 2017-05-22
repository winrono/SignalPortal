

using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SignalmanPortal.Migrations
{
    public partial class AddMigrationsqlmigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TABLE [dbo].[Books] DROP CONSTRAINT FK_Books_CategoriesOfBooks_CategoryId");
            migrationBuilder.Sql(@"ALTER TABLE [dbo].[Books]  WITH CHECK ADD  CONSTRAINT [FK_Books_CategoriesOfBooks_CategoryId] FOREIGN KEY([CategoryId])
                                            REFERENCES[dbo].[CategoriesOfBooks]([CategoryId])
                                            ON DELETE SET NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TABLE [dbo].[Books] DROP CONSTRAINT FK_Books_CategoriesOfBooks_CategoryId");
            migrationBuilder.Sql(@"ALTER TABLE [dbo].[Books]  WITH CHECK ADD  CONSTRAINT [FK_Books_CategoriesOfBooks_CategoryId] FOREIGN KEY([CategoryId])
                                            REFERENCES[dbo].[CategoriesOfBooks]([CategoryId])");
        }
    }
}
