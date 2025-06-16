using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nasteafy.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCoverUrlToMediaEntitiesMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FilePath",
                schema: "music",
                table: "Tracks");

            migrationBuilder.AlterColumn<string>(
                name: "AvatarUrl",
                schema: "auth",
                table: "Users",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CoverUrl",
                schema: "music",
                table: "Tracks",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CoverUrl",
                schema: "music",
                table: "Playlists",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CoverUrl",
                schema: "music",
                table: "Albums",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoverUrl",
                schema: "music",
                table: "Tracks");

            migrationBuilder.DropColumn(
                name: "CoverUrl",
                schema: "music",
                table: "Playlists");

            migrationBuilder.DropColumn(
                name: "CoverUrl",
                schema: "music",
                table: "Albums");

            migrationBuilder.AlterColumn<string>(
                name: "AvatarUrl",
                schema: "auth",
                table: "Users",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FilePath",
                schema: "music",
                table: "Tracks",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");
        }
    }
}
