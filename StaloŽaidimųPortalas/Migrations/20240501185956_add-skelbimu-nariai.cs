using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StaloŽaidimųPortalas.Migrations
{
    /// <inheritdoc />
    public partial class addskelbimunariai : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SkelbimųNariai",
                columns: table => new
                {
                    SkelbimoId = table.Column<int>(type: "int", nullable: false),
                    NaudotojoId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkelbimųNariai", x => new { x.SkelbimoId, x.NaudotojoId });
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SkelbimųNariai");
        }
    }
}
