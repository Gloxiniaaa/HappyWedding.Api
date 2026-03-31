using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HappyWedding.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Username = table.Column<string>(type: "text", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    Role = table.Column<string>(type: "text", nullable: false),
                    RefreshToken = table.Column<string>(type: "text", nullable: true),
                    RefreshTokenExpiryTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    GoogleId = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Weddings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    UserId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    Name1 = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Name2 = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Date = table.Column<DateTime>(type: "date", nullable: false),
                    Location = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    Tagline = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weddings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExpenseCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Emoji = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    WeddingId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpenseCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExpenseCategories_Weddings_WeddingId",
                        column: x => x.WeddingId,
                        principalTable: "Weddings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Guests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Note = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    SeatCount = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    Confirmed = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    Side = table.Column<string>(type: "text", nullable: false),
                    WeddingId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
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

            migrationBuilder.CreateTable(
                name: "Milestones",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Subtitle = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Date = table.Column<DateTime>(type: "date", nullable: false),
                    Completed = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    Emoji = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    WeddingId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Milestones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Milestones_Weddings_WeddingId",
                        column: x => x.WeddingId,
                        principalTable: "Weddings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WeddingPhotos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PublicId = table.Column<string>(type: "text", nullable: false),
                    ImageUrl = table.Column<string>(type: "text", nullable: false),
                    AspectRatio = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false, defaultValue: "1:1"),
                    Caption = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    WeddingId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeddingPhotos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WeddingPhotos_Weddings_WeddingId",
                        column: x => x.WeddingId,
                        principalTable: "Weddings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExpenseItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    EstimateCost = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    ActualCost = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    Paid = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpenseItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExpenseItems_ExpenseCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "ExpenseCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseCategories_WeddingId",
                table: "ExpenseCategories",
                column: "WeddingId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseItems_CategoryId",
                table: "ExpenseItems",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Guest_Name",
                table: "Guests",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Guest_WeddingId_Side",
                table: "Guests",
                columns: new[] { "WeddingId", "Side" });

            migrationBuilder.CreateIndex(
                name: "IX_Milestones_WeddingId",
                table: "Milestones",
                column: "WeddingId");

            migrationBuilder.CreateIndex(
                name: "IX_WeddingPhotos_WeddingId",
                table: "WeddingPhotos",
                column: "WeddingId");

            migrationBuilder.CreateIndex(
                name: "IX_Weddings_UserId",
                table: "Weddings",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExpenseItems");

            migrationBuilder.DropTable(
                name: "Guests");

            migrationBuilder.DropTable(
                name: "Milestones");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "WeddingPhotos");

            migrationBuilder.DropTable(
                name: "ExpenseCategories");

            migrationBuilder.DropTable(
                name: "Weddings");
        }
    }
}
