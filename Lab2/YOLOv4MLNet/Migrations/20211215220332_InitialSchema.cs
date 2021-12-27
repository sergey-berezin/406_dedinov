using Microsoft.EntityFrameworkCore.Migrations;

namespace Component.Migrations
{
    public partial class InitialSchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Results",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    label = table.Column<string>(type: "TEXT", nullable: true),
                    x1 = table.Column<double>(type: "REAL", nullable: false),
                    x2 = table.Column<double>(type: "REAL", nullable: false),
                    x3 = table.Column<double>(type: "REAL", nullable: false),
                    x4 = table.Column<double>(type: "REAL", nullable: false),
                    ImageResultDBid = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Results", x => x.id);
                    table.ForeignKey(
                        name: "FK_Results_Images_ImageResultDBid",
                        column: x => x.ImageResultDBid,
                        principalTable: "Images",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Results_ImageResultDBid",
                table: "Results",
                column: "ImageResultDBid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Results");
        }
    }
}
