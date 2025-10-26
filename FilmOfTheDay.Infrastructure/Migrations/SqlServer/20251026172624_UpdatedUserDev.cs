using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FilmOfTheDay.Infrastructure.Migrations.SqlServer
{
    /// <inheritdoc />
    public partial class UpdatedUserDev : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ReadNotifications",
                table: "Users",
                type: "bit",
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
