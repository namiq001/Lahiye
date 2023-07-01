using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NOOKX_Project.Migrations
{
    public partial class ProductPrice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Products",
                newName: "OldPrice");

            migrationBuilder.AddColumn<double>(
                name: "NewPrice",
                table: "Products",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "Percentage",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NewPrice",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Percentage",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "OldPrice",
                table: "Products",
                newName: "Price");
        }
    }
}
