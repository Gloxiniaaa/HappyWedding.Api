using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HappyWedding.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddGuest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Guests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Note = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    SeatCount = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    Confirmed = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Side = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    WeddingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Guests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Guests_Weddings_WeddingId",
                        column: x => x.WeddingId,
                        principalTable: "Weddings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Guest_Name",
                table: "Guests",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Guest_WeddingId_Side",
                table: "Guests",
                columns: new[] { "WeddingId", "Side" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Guests");
        }
    }
}
