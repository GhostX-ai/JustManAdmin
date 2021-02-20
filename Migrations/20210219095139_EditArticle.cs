using Microsoft.EntityFrameworkCore.Migrations;

namespace JustManAdmin.Migrations
{
    public partial class EditArticle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImgPath",
                table: "MainCategories");

            migrationBuilder.AddColumn<string>(
                name: "ImgPath",
                table: "Articles",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImgPath",
                table: "Articles");

            migrationBuilder.AddColumn<string>(
                name: "ImgPath",
                table: "MainCategories",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
