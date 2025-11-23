using MediatR;
using WorkoutCatalogService.Features.Plans.DTOs;
using WorkoutCatalogService.Features.PlanWorkouts.DTOS;
using WorkoutCatalogService.Shared.Entites;
using WorkoutCatalogService.Shared.GenericRepos;
using WorkoutCatalogService.Shared.Response;
using WorkoutCatalogService.Shared.Srvieces.Validation;

namespace WorkoutCatalogService.Features.Plans.CQRS.Quries
{
    public record GetPlanbyidQyery(Guid id):IRequest<RequestResponse< PalnToReturnDto>>;

    public class GetPlanbyidQyeryHandler : IRequestHandler<GetPlanbyidQyery, RequestResponse<PalnToReturnDto>>
    {

        public GetPlanbyidQyeryHandler(IGenericRepository<Plan> genericRepository)
        {
            GenericRepository = genericRepository;
        }

        public IGenericRepository<Plan> GenericRepository { get; }

        public async Task<RequestResponse<PalnToReturnDto>> Handle(GetPlanbyidQyery request, CancellationToken cancellationToken)
        {
            if (request.id == Guid.Empty)
            {
                return RequestResponse<PalnToReturnDto>.Fail("Plan ID cannot be empty.", 400);
            }
            var plan = GenericRepository.GetAll().Where(p => p.Id == request.id).Select(x => new PalnToReturnDto
            {
                Name = x.Name,
                Description = x.Description,
                DifficultyLevel = x.DifficultyLevel.ToString(),
                AssignedUserIds = x.AssignedUserIds,
                PlanWorkout = x.PlanWorkout.Select(pw => new PlanWorkoutToReturnDto
                {
                    Sets = pw.Sets,
                    Reps = pw.Reps,
                }).ToList()
            }).FirstOrDefault();

            if (plan == null)
            {
                return RequestResponse<PalnToReturnDto>.Fail($"Plan with ID {request.id} not found.", 404);
            }

            return RequestResponse<PalnToReturnDto>.Success(plan, "Plan retrieved successfully", 200);
        }
    }




}
