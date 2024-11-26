using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketPortalMVC.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRating : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "EventRatings",
                type: "longtext",
                nullable: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "EventRatings");
        }
    }
}
