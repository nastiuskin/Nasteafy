using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nasteafy.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveArtistUserRequiredDependencyMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoverUrl",
                schema: "music",
                table: "Tracks");

            migrationBuilder.AddColumn<string>(
                name: "FilePath",
                schema: "music",
                table: "Tracks",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                schema: "music",
                table: "Artists",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<string>(
                name: "AvatarUrl",
                schema: "music",
                table: "Artists",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "CreatedByAdmin",
                schema: "music",
                table: "Artists",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FilePath",
                schema: "music",
                table: "Tracks");

            migrationBuilder.DropColumn(
                name: "AvatarUrl",
                schema: "music",
                table: "Artists");

            migrationBuilder.DropColumn(
                name: "CreatedByAdmin",
                schema: "music",
                table: "Artists");

            migrationBuilder.AddColumn<string>(
                name: "CoverUrl",
                schema: "music",
                table: "Tracks",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                schema: "music",
                table: "Artists",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);
        }
    }
}
