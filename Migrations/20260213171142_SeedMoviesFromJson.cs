using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace tp.Migrations
{
    /// <inheritdoc />
    public partial class SeedMoviesFromJson : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Movies",
                columns: new[] { "Id", "DateTimeMovie", "GenreId", "ImageFile", "Name" },
                values: new object[,]
                {
                    { 101, new DateTime(1994, 9, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "The Shawshank Redemption" },
                    { 102, new DateTime(1972, 3, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "The Godfather" },
                    { 103, new DateTime(1994, 10, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Pulp Fiction" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 101);

            migrationBuilder.DeleteData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 102);

            migrationBuilder.DeleteData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 103);
        }
    }
}
