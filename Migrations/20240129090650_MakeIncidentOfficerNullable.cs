using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartAlertAPI.Migrations
{
    /// <inheritdoc />
    public partial class MakeIncidentOfficerNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Incident_User_AcceptedById",
                table: "Incident");

            migrationBuilder.AlterColumn<Guid>(
                name: "AcceptedById",
                table: "Incident",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_Incident_User_AcceptedById",
                table: "Incident",
                column: "AcceptedById",
                principalTable: "User",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Incident_User_AcceptedById",
                table: "Incident");

            migrationBuilder.AlterColumn<Guid>(
                name: "AcceptedById",
                table: "Incident",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Incident_User_AcceptedById",
                table: "Incident",
                column: "AcceptedById",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
