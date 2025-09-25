using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarDeadlineTracker.Migrations
{
    /// <inheritdoc />
    public partial class MergedDocumentAndMaintenance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documents_Cars_CarNumberPlate",
                table: "Documents");

            migrationBuilder.DropTable(
                name: "MaintenanceRecords");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Documents",
                table: "Documents");

            migrationBuilder.RenameTable(
                name: "Documents",
                newName: "RenewalItems");

            migrationBuilder.RenameColumn(
                name: "RepairType",
                table: "RepairLogs",
                newName: "RepairName");

            migrationBuilder.RenameColumn(
                name: "RenewalDate",
                table: "RenewalItems",
                newName: "ItemName");

            migrationBuilder.RenameColumn(
                name: "RenewalCost",
                table: "RenewalItems",
                newName: "DateofExpiry");

            migrationBuilder.RenameColumn(
                name: "ExpirationDate",
                table: "RenewalItems",
                newName: "DateOfRenewal");

            migrationBuilder.RenameColumn(
                name: "DocumentName",
                table: "RenewalItems",
                newName: "Cost");

            migrationBuilder.RenameIndex(
                name: "IX_Documents_CarNumberPlate",
                table: "RenewalItems",
                newName: "IX_RenewalItems_CarNumberPlate");

            migrationBuilder.AddColumn<decimal>(
                name: "Cost",
                table: "RepairLogs",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "Mileage",
                table: "Cars",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RenewalItems",
                table: "RenewalItems",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RenewalItems_Cars_CarNumberPlate",
                table: "RenewalItems",
                column: "CarNumberPlate",
                principalTable: "Cars",
                principalColumn: "NumberPlate",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RenewalItems_Cars_CarNumberPlate",
                table: "RenewalItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RenewalItems",
                table: "RenewalItems");

            migrationBuilder.DropColumn(
                name: "Cost",
                table: "RepairLogs");

            migrationBuilder.DropColumn(
                name: "Mileage",
                table: "Cars");

            migrationBuilder.RenameTable(
                name: "RenewalItems",
                newName: "Documents");

            migrationBuilder.RenameColumn(
                name: "RepairName",
                table: "RepairLogs",
                newName: "RepairType");

            migrationBuilder.RenameColumn(
                name: "ItemName",
                table: "Documents",
                newName: "RenewalDate");

            migrationBuilder.RenameColumn(
                name: "DateofExpiry",
                table: "Documents",
                newName: "RenewalCost");

            migrationBuilder.RenameColumn(
                name: "DateOfRenewal",
                table: "Documents",
                newName: "ExpirationDate");

            migrationBuilder.RenameColumn(
                name: "Cost",
                table: "Documents",
                newName: "DocumentName");

            migrationBuilder.RenameIndex(
                name: "IX_RenewalItems_CarNumberPlate",
                table: "Documents",
                newName: "IX_Documents_CarNumberPlate");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Documents",
                table: "Documents",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "MaintenanceRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CarNumberPlate = table.Column<string>(type: "TEXT", nullable: false),
                    MaintenanceDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    MaintenanceName = table.Column<string>(type: "TEXT", nullable: false),
                    NextMaintenanceDueDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Notes = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaintenanceRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaintenanceRecords_Cars_CarNumberPlate",
                        column: x => x.CarNumberPlate,
                        principalTable: "Cars",
                        principalColumn: "NumberPlate",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceRecords_CarNumberPlate",
                table: "MaintenanceRecords",
                column: "CarNumberPlate");

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_Cars_CarNumberPlate",
                table: "Documents",
                column: "CarNumberPlate",
                principalTable: "Cars",
                principalColumn: "NumberPlate",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
