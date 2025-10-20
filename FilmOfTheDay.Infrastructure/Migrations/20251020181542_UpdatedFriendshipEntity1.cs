using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FilmOfTheDay.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedFriendshipEntity1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FriendId",
                table: "Friendships");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Friendships");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FriendId",
                table: "Friendships",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Friendships",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
