using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthJournal.Api.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JournalUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ExtUserId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JournalUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JournalWeeks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Start = table.Column<DateOnly>(type: "date", nullable: false),
                    End = table.Column<DateOnly>(type: "date", nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    JournalUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    WeekOfYear_Week = table.Column<int>(type: "integer", nullable: false),
                    WeekOfYear_Year = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JournalWeeks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JournalWeeks_JournalUsers_JournalUserId",
                        column: x => x.JournalUserId,
                        principalTable: "JournalUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JournalEntries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    JournalWeekId = table.Column<Guid>(type: "uuid", nullable: false),
                    EntryType = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    PerformedAt = table.Column<DateOnly>(type: "date", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JournalEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JournalEntries_JournalWeeks_JournalWeekId",
                        column: x => x.JournalWeekId,
                        principalTable: "JournalWeeks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "JournalUsers",
                columns: new[] { "Id", "CreatedAt", "ExtUserId" },
                values: new object[] { new Guid("6c23bc95-9c5f-4ff6-888c-3b2eccf766f2"), new DateTime(2026, 9, 9, 12, 10, 10, 0, DateTimeKind.Utc), "dummy-user" });

            migrationBuilder.CreateIndex(
                name: "IX_JournalEntries_JournalWeekId",
                table: "JournalEntries",
                column: "JournalWeekId");

            migrationBuilder.CreateIndex(
                name: "IX_JournalWeeks_JournalUserId_WeekOfYear",
                table: "JournalWeeks",
                columns: new[] { "JournalUserId", "Start" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JournalEntries");

            migrationBuilder.DropTable(
                name: "JournalWeeks");

            migrationBuilder.DropTable(
                name: "JournalUsers");
        }
    }
}
