namespace WorkoutCatalogService.Features.Workout.DTOs
{
    public class GetRecomendedWorkoutsDto
    {
        public Guid planid { get; set; }

        public string? subcategoryname { get; set; }=null;
    }
}
