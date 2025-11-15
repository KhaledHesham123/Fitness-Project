using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkoutCatalogService.Data.Migrations
{
    /// <inheritdoc />
    public partial class create3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exercises_SubCategory_SubCategoryId",
                table: "Exercises");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutExercise_Exercises_ExerciseId",
                table: "WorkoutExercise");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutExercise_WorkoutPlans_WorkoutPlanId",
                table: "WorkoutExercise");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkoutPlans",
                table: "WorkoutPlans");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkoutExercise",
                table: "WorkoutExercise");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Exercises",
                table: "Exercises");

            migrationBuilder.RenameTable(
                name: "WorkoutPlans",
                newName: "Plan");

            migrationBuilder.RenameTable(
                name: "WorkoutExercise",
                newName: "PlanWorkout");

            migrationBuilder.RenameTable(
                name: "Exercises",
                newName: "Workout");

            migrationBuilder.RenameIndex(
                name: "IX_WorkoutExercise_ExerciseId",
                table: "PlanWorkout",
                newName: "IX_PlanWorkout_ExerciseId");

            migrationBuilder.RenameIndex(
                name: "IX_Exercises_SubCategoryId",
                table: "Workout",
                newName: "IX_Workout_SubCategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Plan",
                table: "Plan",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlanWorkout",
                table: "PlanWorkout",
                columns: new[] { "WorkoutPlanId", "ExerciseId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Workout",
                table: "Workout",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PlanWorkout_Plan_WorkoutPlanId",
                table: "PlanWorkout",
                column: "WorkoutPlanId",
                principalTable: "Plan",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlanWorkout_Workout_ExerciseId",
                table: "PlanWorkout",
                column: "ExerciseId",
                principalTable: "Workout",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Workout_SubCategory_SubCategoryId",
                table: "Workout",
                column: "SubCategoryId",
                principalTable: "SubCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlanWorkout_Plan_WorkoutPlanId",
                table: "PlanWorkout");

            migrationBuilder.DropForeignKey(
                name: "FK_PlanWorkout_Workout_ExerciseId",
                table: "PlanWorkout");

            migrationBuilder.DropForeignKey(
                name: "FK_Workout_SubCategory_SubCategoryId",
                table: "Workout");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Workout",
                table: "Workout");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlanWorkout",
                table: "PlanWorkout");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Plan",
                table: "Plan");

            migrationBuilder.RenameTable(
                name: "Workout",
                newName: "Exercises");

            migrationBuilder.RenameTable(
                name: "PlanWorkout",
                newName: "WorkoutExercise");

            migrationBuilder.RenameTable(
                name: "Plan",
                newName: "WorkoutPlans");

            migrationBuilder.RenameIndex(
                name: "IX_Workout_SubCategoryId",
                table: "Exercises",
                newName: "IX_Exercises_SubCategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_PlanWorkout_ExerciseId",
                table: "WorkoutExercise",
                newName: "IX_WorkoutExercise_ExerciseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Exercises",
                table: "Exercises",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkoutExercise",
                table: "WorkoutExercise",
                columns: new[] { "WorkoutPlanId", "ExerciseId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkoutPlans",
                table: "WorkoutPlans",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Exercises_SubCategory_SubCategoryId",
                table: "Exercises",
                column: "SubCategoryId",
                principalTable: "SubCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutExercise_Exercises_ExerciseId",
                table: "WorkoutExercise",
                column: "ExerciseId",
                principalTable: "Exercises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutExercise_WorkoutPlans_WorkoutPlanId",
                table: "WorkoutExercise",
                column: "WorkoutPlanId",
                principalTable: "WorkoutPlans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
