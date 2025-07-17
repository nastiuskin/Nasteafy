using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nasteafy.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddArtistBiographyMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Biography",
                schema: "music",
                table: "Artists",
                type: "character varying(10000)",
                maxLength: 10000,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Biography",
                schema: "music",
                table: "Artists");
        }
    }
}
