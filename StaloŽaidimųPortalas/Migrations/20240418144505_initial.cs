using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StaloŽaidimųPortalas.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StaloŽaidimai",
                columns: table => new
                {
                    ŽaidimoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Pavadinimas = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Kategorija = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ŽaidėjųKiekisMin = table.Column<int>(type: "int", nullable: true),
                    ŽaidėjųKiekisMax = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaloŽaidimai", x => x.ŽaidimoId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StaloŽaidimai");
        }
    }
}
