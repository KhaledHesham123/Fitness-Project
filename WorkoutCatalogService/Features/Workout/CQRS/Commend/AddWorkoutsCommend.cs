using MediatR;
using WorkoutCatalogService.Features.Categories.DTOs;
using WorkoutCatalogService.Features.Workout.DTOs;
using WorkoutCatalogService.Shared.Entites;
using WorkoutCatalogService.Shared.GenericRepos;
using WorkoutCatalogService.Shared.Response;

namespace WorkoutCatalogService.Features.Workout.CQRS.Commend
{
    public record AddWorkoutsCommend(IEnumerable< WorkoutToaddDto> AddWorkoutDto):IRequest<RequestResponse<IEnumerable<WorkoutToreturnDto>>>;

    public class AddWorkoutCommendHandler:IRequestHandler<AddWorkoutsCommend,RequestResponse<IEnumerable<WorkoutToreturnDto>>>
    {
        private readonly IGenericRepository<Shared.Entites.Workout> genericRepository;

        public AddWorkoutCommendHandler(IGenericRepository<WorkoutCatalogService.Shared.Entites.Workout> genericRepository)
        {
            this.genericRepository = genericRepository;
        }
        public async Task<RequestResponse<IEnumerable<WorkoutToreturnDto>>> Handle(AddWorkoutsCommend request, CancellationToken cancellationToken)
        {
            if (request.AddWorkoutDto==null)
            {
                return RequestResponse<IEnumerable<WorkoutToreturnDto>>.Fail("error while adding Workout",400);
            }


            var workouts = request.AddWorkoutDto.Select(x => new WorkoutCatalogService.Shared.Entites.Workout
            {
                Id = x.id,
                Name = x.Name,
                Description = x.Description,
                DifficultyLevel = Enum.Parse<DifficultyLevel>(x.DifficultyLevel, true),
                MuscleGroup = Enum.Parse<MuscleGroup>(x.MuscleGroup, true),
                DurationMinutes = x.DurationMinutes,
                SubCategoryId = x.SubCategoryId
            }).ToList();

            var result = workouts.Select(w => new WorkoutToreturnDto
            {
                id = w.Id,
                Name = w.Name,
                Description = w.Description,
                DifficultyLevel = w.DifficultyLevel.ToString(),
                MuscleGroup = w.MuscleGroup.ToString(),
                DurationMinutes = w.DurationMinutes,
                SubCategoryId = w.SubCategoryId,
                
            });

            await genericRepository.AddRangeAsync(workouts);
            await genericRepository.SaveChanges();

            return RequestResponse<IEnumerable<WorkoutToreturnDto>>.Success(result, "Workout added successfully", 200);
        }
    }


}
