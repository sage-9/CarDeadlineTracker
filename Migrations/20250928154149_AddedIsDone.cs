using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarDeadlineTracker.Migrations
{
    /// <inheritdoc />
    public partial class AddedIsDone : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDone",
                table: "RenewalItems",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDone",
                table: "RenewalItems");
        }
    }
}
