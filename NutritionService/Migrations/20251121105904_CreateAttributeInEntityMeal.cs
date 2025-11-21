using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NutritionService.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateAttributeInEntityMeal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isPremium",
                table: "Meals",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Meals",
                keyColumn: "Id",
                keyValue: 1,
                column: "isPremium",
                value: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isPremium",
                table: "Meals");
        }
    }
}
