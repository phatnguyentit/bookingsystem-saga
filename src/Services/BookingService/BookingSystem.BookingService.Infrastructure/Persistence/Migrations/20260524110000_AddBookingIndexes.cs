using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingSystem.BookingService.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddBookingIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_bookings_check_in",
                table: "bookings",
                column: "check_in");

            migrationBuilder.CreateIndex(
                name: "IX_bookings_check_out",
                table: "bookings",
                column: "check_out");

            migrationBuilder.CreateIndex(
                name: "IX_bookings_status",
                table: "bookings",
                column: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_bookings_check_in",
                table: "bookings");

            migrationBuilder.DropIndex(
                name: "IX_bookings_check_out",
                table: "bookings");

            migrationBuilder.DropIndex(
                name: "IX_bookings_status",
                table: "bookings");
        }
    }
}
