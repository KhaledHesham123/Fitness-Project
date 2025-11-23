using MediatR;
using WorkoutCatalogService.Features.PlanWorkouts.DTOS;
using WorkoutCatalogService.Shared.Entites;
using WorkoutCatalogService.Shared.GenericRepos;
using WorkoutCatalogService.Shared.Response;
using WorkoutCatalogService.Shared.Srvieces.Validation;

namespace WorkoutCatalogService.Features.PlanWorkouts.CQRS.Commends
{
    public record AddPlanWorkoutCommend(IEnumerable<AddPlanWorkoutDto> PlanWorkoutDtos) :IRequest<RequestResponse< IEnumerable< Guid>>>;

    public class AddPlanWorkoutCommendHandler : IRequestHandler<AddPlanWorkoutCommend, RequestResponse<IEnumerable<Guid>>>
    {
        private readonly IGenericRepository<PlanWorkout> _genericRepository;

        public AddPlanWorkoutCommendHandler(IGenericRepository<PlanWorkout> genericRepository)
        {
            this._genericRepository = genericRepository;
        }
        public async Task<RequestResponse<IEnumerable<Guid>>> Handle(AddPlanWorkoutCommend request, CancellationToken cancellationToken)
        {
            bool isvalid= RequestValidator<AddPlanWorkoutDto>.TryValidate(request.PlanWorkoutDtos, out List<string> errors);
            if (!isvalid)
                return RequestResponse<IEnumerable<Guid>>.Fail(string.Join(", ", errors),400);

            var PLanWorkouts= request.PlanWorkoutDtos.Select(dto => new PlanWorkout
            {
               Sets=dto.Sets,
               Reps=dto.Reps,
               ExerciseId=dto.ExerciseId,
               WorkoutPlanId=dto.WorkoutPlanId
            }).ToList();

            await _genericRepository.AddRangeAsync(PLanWorkouts);
            await _genericRepository.SaveChanges();



            return RequestResponse<IEnumerable<Guid>>.Success(PLanWorkouts.Select(pw=>pw.Id), "PlanWorkouts added successfully", 201);

        }
    }
}
