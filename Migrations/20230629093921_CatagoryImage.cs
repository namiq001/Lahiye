using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NOOKX_Project.Migrations
{
    public partial class CatagoryImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageName",
                table: "Catagories",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageName",
                table: "Catagories");
        }
    }
}
