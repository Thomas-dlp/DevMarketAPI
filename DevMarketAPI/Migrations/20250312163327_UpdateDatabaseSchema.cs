using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevMarketAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDatabaseSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "email",
                table: "StudioCredentials",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "password",
                table: "StudioCredentials",
                newName: "PasswordHash");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "StudioCredentials",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "StudioCredentials");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "StudioCredentials",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "PasswordHash",
                table: "StudioCredentials",
                newName: "password");
        }
    }
}
