using MediatR;
using WorkoutCatalogService.Features.Plans.CQRS.Commends;
using WorkoutCatalogService.Features.Plans.DTOs;
using WorkoutCatalogService.Features.PlanWorkouts.CQRS.Commends;
using WorkoutCatalogService.Features.PlanWorkouts.DTOS;
using WorkoutCatalogService.Shared.Entites;
using WorkoutCatalogService.Shared.Response;

namespace WorkoutCatalogService.Features.Plans.CQRS.Orchestrator
{
    public record AddPlanOrchestrator(AddplanDto AddplanDto,IEnumerable< AddPlanWorkoutDto> AddPlanWorkoutDto) :IRequest<RequestResponse<PalnToReturnDto>>;

    public class AddPlanOrchestratorHandler : IRequestHandler<AddPlanOrchestrator, RequestResponse<PalnToReturnDto>>
    {
        private readonly IMediator _mediator;

        public AddPlanOrchestratorHandler(IMediator mediator)
        {
            this._mediator = mediator;
        }
        public async Task<RequestResponse<PalnToReturnDto>> Handle(AddPlanOrchestrator request, CancellationToken cancellationToken)
        {
            if (request.AddPlanWorkoutDto==null&&request.AddplanDto==null)
            {
                return RequestResponse<PalnToReturnDto>.Fail("Something went wrong during adding Plan.", 400);
            }

            var planWorkouts= await _mediator.Send(new AddPlanWorkoutCommend(request.AddPlanWorkoutDto));

            var plan = await _mediator.Send(new AddPlanCommend(request.AddplanDto,planWorkouts));

            var planDto = new PalnToReturnDto
            {
                Name = plan.Data.Name,
                Description = plan.Data.Description,
                DifficultyLevel = plan.Data.DifficultyLevel,
                AssignedUserIds = plan.Data.AssignedUserIds,
                PlanWorkout = plan.Data.PlanWorkout.Select(pw => new PlanWorkoutToReturnDto
                {
                    Id = pw.Id,
                    Sets = pw.Sets,
                    Reps = pw.Reps,
                    WorkoutName = "TODO: احصل على اسم الـ Workout لو محتاج"
                }).ToList()
            };

            return RequestResponse<PalnToReturnDto>.Success(planDto, "Plan added successfully", 200);


        }
    }
}
