using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevMarketAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddReflink : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StudiioId",
                table: "Posts");

            migrationBuilder.AlterColumn<Guid>(
                name: "StudioId",
                table: "TradingStatuses",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<Guid>(
                name: "StudioId",
                table: "Posts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "ReferenceLinks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudioId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DisplayableElementId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DisplayableElementType = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReferenceLinks", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReferenceLinks_StudioId",
                table: "ReferenceLinks",
                column: "StudioId");

            migrationBuilder.CreateIndex(
                name: "IX_ReferenceLinks_StudioId_DisplayableElementId_DisplayableElementType",
                table: "ReferenceLinks",
                columns: new[] { "StudioId", "DisplayableElementId", "DisplayableElementType" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReferenceLinks");

            migrationBuilder.DropColumn(
                name: "StudioId",
                table: "Posts");

            migrationBuilder.AlterColumn<string>(
                name: "StudioId",
                table: "TradingStatuses",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<string>(
                name: "StudiioId",
                table: "Posts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
