using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nasteafy.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddAvatarUrlMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AvatarUrl",
                schema: "auth",
                table: "Users",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvatarUrl",
                schema: "auth",
                table: "Users");
        }
    }
}
