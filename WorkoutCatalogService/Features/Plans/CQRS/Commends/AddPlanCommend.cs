using MediatR;
using System.Numerics;
using WorkoutCatalogService.Features.Plans.DTOs;
using WorkoutCatalogService.Shared.Entites;
using WorkoutCatalogService.Shared.GenericRepos;
using WorkoutCatalogService.Shared.Response;

namespace WorkoutCatalogService.Features.Plans.CQRS.Commends
{
    public record AddPlanCommend(AddplanDto AddplanDto,IEnumerable<PlanWorkout> PlanWorkouts ):IRequest<RequestResponse<Plan>>;

    public class AddPlanCommendHandler : IRequestHandler<AddPlanCommend, RequestResponse<Plan>>
    {
        private readonly IGenericRepository<Shared.Entites.Plan> _genericRepository;

        public AddPlanCommendHandler(IGenericRepository<WorkoutCatalogService.Shared.Entites.Plan> genericRepository)
        {
            this._genericRepository = genericRepository;
        }
        public async Task<RequestResponse<Plan>> Handle(AddPlanCommend request, CancellationToken cancellationToken)
        {
            if (request.AddplanDto == null)
            {
                return RequestResponse<Plan>.Fail("Something went wrong during adding Plan.",400);
            }

            var plan = new Plan 
            {
                Id=request.AddplanDto.id,
                Description=request.AddplanDto.Description,
                Name=request.AddplanDto.Name,
                DifficultyLevel=request.AddplanDto.DifficultyLevel,
                PlanWorkout=request.PlanWorkouts.ToList()
               
                
            };

            await _genericRepository.addAsync(plan);
            await _genericRepository.SaveChanges();

            return RequestResponse<Plan>.Success(plan, "Plan added successfully", 200);

        }
    }
}
