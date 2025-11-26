using MediatR;
using Microsoft.EntityFrameworkCore;
using WorkoutCatalogService.Shared.Entites;
using WorkoutCatalogService.Shared.GenericRepos;
using WorkoutCatalogService.Shared.Response;

namespace WorkoutCatalogService.Features.PlanWorkouts.CQRS.Quries
{
    public record GetUserWorkoutIdsBuPlanIdQuery(Guid planid) : IRequest<RequestResponse<IEnumerable<Guid>>>;

    public class GetUserWorkoutIdsBuPlanIdQueryHandler : IRequestHandler<GetUserWorkoutIdsBuPlanIdQuery, RequestResponse<IEnumerable<Guid>>>
    {
        private readonly IGenericRepository<PlanWorkout> genericRepository;

        public GetUserWorkoutIdsBuPlanIdQueryHandler(IGenericRepository<PlanWorkout> genericRepository)
        {
            this.genericRepository = genericRepository;
        }
        public async Task<RequestResponse<IEnumerable<Guid>>> Handle(GetUserWorkoutIdsBuPlanIdQuery request, CancellationToken cancellationToken)
        {
            var workoutIds = await genericRepository
                    .GetAll()
                    .Where(pw => pw.WorkoutPlanId == request.planid)
                    .Select(pw => pw.ExerciseId)
                    .ToListAsync(cancellationToken);

            if (!workoutIds.Any())
            {
                return RequestResponse<IEnumerable<Guid>>.Fail("there is no Workouts in this plan", 400); 
            }
            return RequestResponse<IEnumerable<Guid>>.Success(workoutIds, "Workout Ids retrieved successfully", 200);
        }
    }
}
