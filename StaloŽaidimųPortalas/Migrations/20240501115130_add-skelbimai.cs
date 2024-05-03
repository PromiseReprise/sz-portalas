using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StaloŽaidimųPortalas.Migrations
{
    /// <inheritdoc />
    public partial class addskelbimai : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Pavadinimas",
                table: "StaloŽaidimai",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Kategorija",
                table: "StaloŽaidimai",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "Skelbimai",
                columns: table => new
                {
                    SkelbimoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Pavadinimas = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    NaudotojoId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    ŽaidimoId = table.Column<int>(type: "int", nullable: false),
                    Aprašymas = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skelbimai", x => x.SkelbimoId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Skelbimai");

            migrationBuilder.AlterColumn<string>(
                name: "Pavadinimas",
                table: "StaloŽaidimai",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Kategorija",
                table: "StaloŽaidimai",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250,
                oldNullable: true);
        }
    }
}
