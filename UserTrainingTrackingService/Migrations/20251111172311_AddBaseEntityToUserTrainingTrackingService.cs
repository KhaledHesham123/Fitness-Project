using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserTrainingTrackingService.Migrations
{
    /// <inheritdoc />
    public partial class AddBaseEntityToUserTrainingTrackingService : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "WorkoutExerciseCompletions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "UserWorkoutSessions",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "WorkoutExerciseCompletions");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "UserWorkoutSessions");
        }
    }
}
