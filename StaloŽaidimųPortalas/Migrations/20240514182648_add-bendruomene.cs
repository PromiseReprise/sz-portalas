using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StaloŽaidimųPortalas.Migrations
{
    /// <inheritdoc />
    public partial class addbendruomene : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bendruomenė",
                columns: table => new
                {
                    BendruomenėsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NaudotojoId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    ŽaidimoId = table.Column<int>(type: "int", nullable: false),
                    Aprašymas = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bendruomenė", x => x.BendruomenėsId);
                });

            migrationBuilder.CreateTable(
                name: "BendruomenėsĮrašas",
                columns: table => new
                {
                    ĮrašoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BendruomenėsId = table.Column<int>(type: "int", nullable: false),
                    Įrašas = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BendruomenėsĮrašas", x => x.ĮrašoId);
                });

            migrationBuilder.CreateTable(
                name: "BendruomenėsNarys",
                columns: table => new
                {
                    BendruomenėsId = table.Column<int>(type: "int", nullable: false),
                    NaudotojoId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    ArAdministratorius = table.Column<bool>(type: "bit", nullable: false),
                    ArAktyvus = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BendruomenėsNarys", x => new { x.BendruomenėsId, x.NaudotojoId });
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bendruomenė");

            migrationBuilder.DropTable(
                name: "BendruomenėsĮrašas");

            migrationBuilder.DropTable(
                name: "BendruomenėsNarys");
        }
    }
}
