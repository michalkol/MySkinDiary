using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Diary.Migrations
{
    /// <inheritdoc />
    public partial class AddedSeedingDataDiaryEntry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "DiaryEntries",
                columns: new[] { "Id", "Content", "Created", "Title" },
                values: new object[,]
                {
                    { 1, "Today I went hiking w/ my friends.", new DateTime(2024, 11, 9, 15, 41, 29, 936, DateTimeKind.Local).AddTicks(7407), "Hiking" },
                    { 2, "Today I went swimming w/ my friends.", new DateTime(2024, 11, 9, 15, 41, 29, 936, DateTimeKind.Local).AddTicks(7409), "Swimming" },
                    { 3, "Today I went shopping w/ my friends.", new DateTime(2024, 11, 9, 15, 41, 29, 936, DateTimeKind.Local).AddTicks(7411), "Sshopping" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "DiaryEntries",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "DiaryEntries",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "DiaryEntries",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
