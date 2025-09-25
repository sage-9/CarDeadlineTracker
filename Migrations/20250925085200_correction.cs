using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarDeadlineTracker.Migrations
{
    /// <inheritdoc />
    public partial class correction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateofExpiry",
                table: "RenewalItems",
                newName: "DateOfExpiry");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateOfExpiry",
                table: "RenewalItems",
                newName: "DateofExpiry");
        }
    }
}
