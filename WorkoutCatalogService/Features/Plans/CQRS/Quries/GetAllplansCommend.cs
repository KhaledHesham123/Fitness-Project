using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Numerics;
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
            var plans = await genericRepository.GetAll().Select(x => new PalnToReturnDto
            {
                Name = x.Name,
                Description = x.Description,
                DifficultyLevel = x.DifficultyLevel.ToString(),
                AssignedUserIds = x.AssignedUserIds,

                PlanWorkout = x.PlanWorkout.Select(pw => new PlanWorkoutToReturnDto
                {
                    Sets = pw.Sets,
                    Reps = pw.Reps,
                }).ToList()}).ToListAsync();


            if (!plans.Any())
                return RequestResponse<IEnumerable<PalnToReturnDto>>.Fail("there is no plans",400);

            
            return RequestResponse<IEnumerable<PalnToReturnDto>>.Success(plans, "Plans retrieved successfully", 200);

        }
    }
}
