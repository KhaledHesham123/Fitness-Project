using MediatR;
using Microsoft.EntityFrameworkCore;
using WorkoutCatalogService.Features.Workout.DTOs;
using WorkoutCatalogService.Shared.Entites;
using WorkoutCatalogService.Shared.GenericRepos;
using WorkoutCatalogService.Shared.Response;

namespace WorkoutCatalogService.Features.Workout.CQRS.Queries
{
    public record GetRecommendedWorkoutsQuery(IEnumerable<Guid> Workoutids,string? subcategoryName=null): IRequest<RequestResponse<IEnumerable<RecomendedWorkoutsDto>>>;

    public class GetRecommendedWorkoutsQueryHandler:IRequestHandler<GetRecommendedWorkoutsQuery,RequestResponse<IEnumerable<RecomendedWorkoutsDto>>>
    {
        private readonly IGenericRepository<Shared.Entites.Workout> genericRepository;

        public GetRecommendedWorkoutsQueryHandler(IGenericRepository<WorkoutCatalogService.Shared.Entites.Workout> genericRepository)
        {
            this.genericRepository = genericRepository;
        }
        public async Task<RequestResponse<IEnumerable<RecomendedWorkoutsDto>>> Handle(GetRecommendedWorkoutsQuery request, CancellationToken cancellationToken)
        {
            var query = genericRepository.GetAll()
                .Where(w => !request.Workoutids.Contains(w.Id) && !w.IsDeleted);

            if (!string.IsNullOrEmpty(request.subcategoryName))
            {
                query = query.Where(w => w.SubCategory.Name == request.subcategoryName);
            }

            var workouts = await query
                .OrderBy(x => Guid.NewGuid()) 
                .Take(5)
                .Select(w => new RecomendedWorkoutsDto
                {
                    Name = w.Name,
                    Description = w.Description,
                    DifficultyLevel = w.DifficultyLevel.ToString(),
                    SubCategoryName = w.SubCategory.Name,
                    DurationMinutes = w.DurationMinutes,
                    
                })
                .ToListAsync();

            if (!workouts.Any())
            {
                return RequestResponse<IEnumerable<RecomendedWorkoutsDto>>.Fail("there is no workouts avalibale", 400);
            }

            return RequestResponse<IEnumerable<RecomendedWorkoutsDto>>.Success(
                workouts,
                "Workouts retrieved successfully");


        }

    }


}
