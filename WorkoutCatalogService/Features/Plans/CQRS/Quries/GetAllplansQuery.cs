using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Numerics;
using System.Text.Json;
using WorkoutCatalogService.Features.Plans.DTOs;
using WorkoutCatalogService.Features.PlanWorkouts.DTOS;
using WorkoutCatalogService.Shared.Entites;
using WorkoutCatalogService.Shared.GenericRepos;
using WorkoutCatalogService.Shared.Response;

namespace WorkoutCatalogService.Features.Plans.CQRS.Quries
{
    public record GetAllplansQuery:IRequest<RequestResponse<IEnumerable<PalnToReturnDto>>>;

    public class GetAllplansQueryHandler : IRequestHandler<GetAllplansQuery, RequestResponse<IEnumerable<PalnToReturnDto>>>
    {
        private readonly IGenericRepository<Plan> _genericRepository;
       

        public GetAllplansQueryHandler(
            IGenericRepository<Plan> genericRepository )
        {
            _genericRepository = genericRepository;
            
        }
        public async Task<RequestResponse<IEnumerable<PalnToReturnDto>>> Handle(GetAllplansQuery request, CancellationToken cancellationToken)
        {
           
            var plans = await _genericRepository.GetAll().Select(x => new PalnToReturnDto
            {
                id=x.Id,
                Name = x.Name,
                Description = x.Description,
                DifficultyLevel = x.DifficultyLevel.ToString(),

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
