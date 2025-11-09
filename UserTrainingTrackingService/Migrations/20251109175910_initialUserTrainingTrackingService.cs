using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserTrainingTrackingService.Migrations
{
    /// <inheritdoc />
    public partial class initialUserTrainingTrackingService : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserWorkoutSessions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    WorkoutPlanId = table.Column<int>(type: "int", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserWorkoutSessions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkoutExerciseCompletions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExerciseId = table.Column<int>(type: "int", nullable: false),
                    UserWorkoutSessionId = table.Column<int>(type: "int", nullable: false),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkoutExerciseCompletions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkoutExerciseCompletions_UserWorkoutSessions_UserWorkoutSessionId",
                        column: x => x.UserWorkoutSessionId,
                        principalTable: "UserWorkoutSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserWorkoutSessions_UserId",
                table: "UserWorkoutSessions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserWorkoutSessions_WorkoutPlanId",
                table: "UserWorkoutSessions",
                column: "WorkoutPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutExerciseCompletions_ExerciseId",
                table: "WorkoutExerciseCompletions",
                column: "ExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutExerciseCompletions_UserWorkoutSessionId",
                table: "WorkoutExerciseCompletions",
                column: "UserWorkoutSessionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkoutExerciseCompletions");

            migrationBuilder.DropTable(
                name: "UserWorkoutSessions");
        }
    }
}
