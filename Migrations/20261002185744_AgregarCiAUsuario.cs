using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SER_Balanza_Interno.Migrations
{
    /// <inheritdoc />
    public partial class AgregarCiAUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ci",
                table: "Usuario",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ci",
                table: "Usuario");
        }
    }
}
