using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartAlertAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddedCiviliProtectionWebURLToDangers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "WebsiteURL",
                table: "DangerCategory",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WebsiteURL",
                table: "DangerCategory");
        }
    }
}
