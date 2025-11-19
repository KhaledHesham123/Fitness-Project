using MediatR;
using Microsoft.EntityFrameworkCore;
using WorkoutCatalogService.Features.Plans.DTOs;
using WorkoutCatalogService.Features.PlanWorkouts.DTOS;
using WorkoutCatalogService.Shared.Entites;
using WorkoutCatalogService.Shared.GenericRepos;
using WorkoutCatalogService.Shared.Response;

namespace WorkoutCatalogService.Features.Plans.CQRS.Quries
{
    public record GetAllplansCommend:IRequest<RequestResponse<IEnumerable<PalnToReturnDto>>>;

    public class GetAllPlansCommendHandler : IRequestHandler<GetAllplansCommend, RequestResponse<IEnumerable<PalnToReturnDto>>>
    {
        private readonly IGenericRepository<Plan> genericRepository;

        public GetAllPlansCommendHandler(IGenericRepository<Plan> genericRepository)
        {
            this.genericRepository = genericRepository;
        }
        public async Task<RequestResponse<IEnumerable<PalnToReturnDto>>> Handle(GetAllplansCommend request, CancellationToken cancellationToken)
        {
            var plans = await genericRepository.GetAll().Include(x=>x.PlanWorkout).ToListAsync();

            if (!plans.Any())
                return RequestResponse<IEnumerable<PalnToReturnDto>>.Fail("there is no plans",400);

            var mappedplan = new List<PalnToReturnDto>();

            var mappedPlans = plans.Select(plan => new PalnToReturnDto
            {
                Name = plan.Name,
                Description = plan.Description,
                DifficultyLevel = plan.DifficultyLevel,
                AssignedUserIds = plan.AssignedUserIds,
                PlanWorkout = plan.PlanWorkout.Select(pw => new PlanWorkoutToReturnDto
                {
                    Id = pw.Id,
                    Sets = pw.Sets,
                    Reps = pw.Reps,
                }).ToList()
            }).ToList();
            return RequestResponse<IEnumerable<PalnToReturnDto>>.Success(mappedPlans, "Plans retrieved successfully", 200);

        }
    }
}
