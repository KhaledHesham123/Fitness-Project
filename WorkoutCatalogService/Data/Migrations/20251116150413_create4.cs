using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkoutCatalogService.Data.Migrations
{
    /// <inheritdoc />
    public partial class create4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AssignedUserIds",
                table: "Plan",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssignedUserIds",
                table: "Plan");
        }
    }
}
