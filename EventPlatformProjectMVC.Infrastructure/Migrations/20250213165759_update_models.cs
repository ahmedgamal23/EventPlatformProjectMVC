using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventPlatformProjectMVC.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class update_models : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropTable(
                name: "Tickets");


            migrationBuilder.RenameColumn(
                name: "TicketID",
                table: "Bookings",
                newName: "TicketCount");

            migrationBuilder.AddColumn<int>(
                name: "AvailableSeats",
                table: "Events",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "Events",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "TotalPrice",
                table: "Bookings",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvailableSeats",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "Bookings");

            migrationBuilder.RenameColumn(
                name: "TicketCount",
                table: "Bookings",
                newName: "TicketID");

            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventID = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tickets_Events_EventID",
                        column: x => x.EventID,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_TicketID",
                table: "Bookings",
                column: "TicketID");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_EventID",
                table: "Tickets",
                column: "EventID");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Tickets_TicketID",
                table: "Bookings",
                column: "TicketID",
                principalTable: "Tickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
