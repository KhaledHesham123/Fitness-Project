using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NutritionService.Data.Migrations
{
    /// <inheritdoc />
    public partial class CraetrNutritionInSqlServerDocker : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Meals",
                columns: new[] { "Id", "Calories", "Carbohydrates", "CreatedAt", "Description", "DifficultyLevel", "Fat", "ImageUrl", "IsDeleted", "MealType", "Name", "Protein", "UpdatedAt", "VideoUrl" },
                values: new object[] { 1, 600, 75.00m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "A classic Italian pasta dish with rich meat sauce.", 2, 20.00m, "https://example.com/images/spaghetti-bolognese.jpg", false, 3, "Spaghetti Bolognese", 25.50m, null, "https://example.com/spaghetti-bolognese" });

            migrationBuilder.InsertData(
                table: "Ingredients",
                columns: new[] { "Id", "CreatedAt", "IsDeleted", "MealId", "Name", "Quantity", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 1, "Oats", "1", null },
                    { 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 1, "Milk", "1", null },
                    { 3, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 1, "Banana", "1", null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Ingredients",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Meals",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
