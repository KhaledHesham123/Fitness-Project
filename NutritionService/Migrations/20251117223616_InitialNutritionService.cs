using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NutritionService.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialNutritionService : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Meals",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Ingredients",
                newName: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Meals",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Ingredients",
                newName: "ID");
        }
    }
}
