using MediatR;
using WorkoutCatalogService.Features.PlanWorkouts.DTOS;
using WorkoutCatalogService.Shared.Entites;
using WorkoutCatalogService.Shared.GenericRepos;
using WorkoutCatalogService.Shared.Response;

namespace WorkoutCatalogService.Features.PlanWorkouts.CQRS.Commends
{
    public record AddPlanWorkoutCommend(IEnumerable<AddPlanWorkoutDto> PlanWorkoutDtos) :IRequest< IEnumerable< PlanWorkout>>;

    public class AddPlanWorkoutCommendHandler : IRequestHandler<AddPlanWorkoutCommend, IEnumerable<PlanWorkout>>
    {
        private readonly IGenericRepository<PlanWorkout> _genericRepository;

        public AddPlanWorkoutCommendHandler(IGenericRepository<PlanWorkout> genericRepository)
        {
            this._genericRepository = genericRepository;
        }
        public async Task<IEnumerable<PlanWorkout>> Handle(AddPlanWorkoutCommend request, CancellationToken cancellationToken)
        {
            if (request.PlanWorkoutDtos == null)
                return null;

            var PLanWorkouts= new List<PlanWorkout>();

            foreach (var planworkout in request.PlanWorkoutDtos)
            {
                PLanWorkouts.Add(new PlanWorkout 
                {
                    Sets = planworkout.Sets,
                    Reps = planworkout.Reps,
                    ExerciseId =planworkout.ExerciseId,
                    WorkoutPlanId=planworkout.WorkoutPlanId,
                });


            }

            if (!PLanWorkouts.Any())
                return null;

            await _genericRepository.AddRangeAsync(PLanWorkouts);
            await _genericRepository.SaveChanges();

            return PLanWorkouts;

        }
    }
}
