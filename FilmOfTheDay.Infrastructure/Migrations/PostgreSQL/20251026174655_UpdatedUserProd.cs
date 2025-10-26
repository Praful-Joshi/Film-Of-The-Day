using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FilmOfTheDay.Infrastructure.Migrations.PostgreSQL
{
    /// <inheritdoc />
    public partial class UpdatedUserProd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ReadNotifications",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReadNotifications",
                table: "Users");
        }
    }
}
