using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartAlertAPI.Migrations
{
    /// <inheritdoc />
    public partial class RemovedTimeForNotification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeForNotification",
                table: "DangerCategory");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TimeForNotification",
                table: "DangerCategory",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
