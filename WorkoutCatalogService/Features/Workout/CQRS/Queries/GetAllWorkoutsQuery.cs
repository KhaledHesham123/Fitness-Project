using MediatR;
using Microsoft.EntityFrameworkCore;
using WorkoutCatalogService.Features.Workout.DTOs;
using WorkoutCatalogService.Shared.Entites;
using WorkoutCatalogService.Shared.GenericRepos;
using WorkoutCatalogService.Shared.Response;

namespace WorkoutCatalogService.Features.Workout.CQRS.Queries
{
    public record GetAllWorkoutsQuery:IRequest<RequestResponse<IEnumerable<WorkoutToreturnDto>>>;
    public class GetAllWorkoutsQueryHandler : IRequestHandler<GetAllWorkoutsQuery, RequestResponse<IEnumerable<WorkoutToreturnDto>>>
    {
        private readonly IGenericRepository<Shared.Entites.Workout> genericRepository;

        public GetAllWorkoutsQueryHandler(IGenericRepository<WorkoutCatalogService.Shared.Entites.Workout> genericRepository)
        {
            this.genericRepository = genericRepository;
        }
        public async Task<RequestResponse<IEnumerable<WorkoutToreturnDto>>> Handle(GetAllWorkoutsQuery request, CancellationToken cancellationToken)
        {
            var workouts = await genericRepository.GetAll().Include(x=>x.SubCategory).ToListAsync();

            if (workouts == null || !workouts.Any())
            {
                return RequestResponse<IEnumerable<WorkoutToreturnDto>>.Fail("No workouts found.", 404);
            }

            var MappedWorkout= workouts.Select(w => new WorkoutToreturnDto
            {
                Name = w.Name,
                Description = w.Description,
                SubCategoryId = w.SubCategoryId,
                SubCategory = new Features.Categories.DTOs.SubCategoryDTo
                {
                    Name = w.SubCategory.Name,
                    Description=w.SubCategory.Description,
                    
                },
                DifficultyLevel = w.DifficultyLevel.ToString(),
                DurationMinutes=w.DurationMinutes,
                MuscleGroup = w.MuscleGroup.ToString(),
            });

            return RequestResponse<IEnumerable<WorkoutToreturnDto>>.Success(MappedWorkout);
        }
    }
}
