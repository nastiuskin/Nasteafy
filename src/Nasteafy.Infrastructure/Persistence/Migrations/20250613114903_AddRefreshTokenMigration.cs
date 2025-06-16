using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nasteafy.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddRefreshTokenMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RefreshToken",
                schema: "auth",
                table: "Users",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RefreshTokenExpires",
                schema: "auth",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RefreshToken",
                schema: "auth",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "RefreshTokenExpires",
                schema: "auth",
                table: "Users");
        }
    }
}
