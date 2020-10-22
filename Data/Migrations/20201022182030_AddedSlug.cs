using Microsoft.EntityFrameworkCore.Migrations;

namespace MVCBlog.Data.Migrations
{
    public partial class AddedSlug : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Abstract",
                table: "Post",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Body",
                table: "Post",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "Post",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Abstract",
                table: "Post");

            migrationBuilder.DropColumn(
                name: "Body",
                table: "Post");

            migrationBuilder.DropColumn(
                name: "Slug",
                table: "Post");
        }
    }
}
