using Microsoft.EntityFrameworkCore.Migrations;

namespace Component.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "name",
                table: "Images",
                newName: "path");

            migrationBuilder.RenameColumn(
                name: "hash",
                table: "Images",
                newName: "hashCode");

            migrationBuilder.RenameColumn(
                name: "blob",
                table: "Images",
                newName: "pic");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "pic",
                table: "Images",
                newName: "blob");

            migrationBuilder.RenameColumn(
                name: "path",
                table: "Images",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "hashCode",
                table: "Images",
                newName: "hash");
        }
    }
}
