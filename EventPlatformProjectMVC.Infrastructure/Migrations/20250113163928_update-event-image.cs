using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventPlatformProjectMVC.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateeventimage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Picture",
                table: "Events",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Picture",
                table: "Events");
        }
    }
}
