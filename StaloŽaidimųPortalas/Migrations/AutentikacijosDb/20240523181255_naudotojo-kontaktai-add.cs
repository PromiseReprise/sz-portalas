using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StaloŽaidimųPortalas.Migrations.AutentikacijosDb
{
    /// <inheritdoc />
    public partial class naudotojokontaktaiadd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContactInformation",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DiscordServerLink",
                table: "AspNetUsers",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContactInformation",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DiscordServerLink",
                table: "AspNetUsers");
        }
    }
}
