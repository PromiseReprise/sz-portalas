using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StaloŽaidimųPortalas.Migrations.AutentikacijosDb
{
    /// <inheritdoc />
    public partial class naudotojolinkremove : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiscordServerLink",
                table: "AspNetUsers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DiscordServerLink",
                table: "AspNetUsers",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);
        }
    }
}
