using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NOOKX_Project.Migrations
{
    public partial class RemoveImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageName",
                table: "Catagories");

            migrationBuilder.DropColumn(
                name: "IsFeatured",
                table: "Catagories");

            migrationBuilder.DropColumn(
                name: "IsReating",
                table: "Catagories");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageName",
                table: "Catagories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsFeatured",
                table: "Catagories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsReating",
                table: "Catagories",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
