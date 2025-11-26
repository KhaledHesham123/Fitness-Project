using MediatR;
using WorkoutCatalogService.Features.PlanWorkouts.CQRS.Quries;
using WorkoutCatalogService.Features.Workout.CQRS.Queries;
using WorkoutCatalogService.Features.Workout.DTOs;
using WorkoutCatalogService.Shared.Response;

namespace WorkoutCatalogService.Features.Workout.CQRS.Orchestrators
{
    public record GetRecommendedWorkoutsByPlan(Guid USerplanid,string? subcategoryName=null) : IRequest<RequestResponse<IEnumerable<RecomendedWorkoutsDto>>>;

    public class GetRecommendedWorkoutsByPlanHandler : IRequestHandler<GetRecommendedWorkoutsByPlan, RequestResponse<IEnumerable<RecomendedWorkoutsDto>>>
    {
        private readonly IMediator mediator;
        public GetRecommendedWorkoutsByPlanHandler(IMediator mediator)
        {
            this.mediator = mediator;
        }
        public async Task<RequestResponse<IEnumerable<RecomendedWorkoutsDto>>> Handle(GetRecommendedWorkoutsByPlan request, CancellationToken cancellationToken)
        {
            var WorkoutIdsByPlanidRespone = await mediator.Send(new GetUserWorkoutIdsBuPlanIdQuery(request.USerplanid));

            if (!WorkoutIdsByPlanidRespone.IsSuccess)
                return RequestResponse<IEnumerable<RecomendedWorkoutsDto>>.Fail(WorkoutIdsByPlanidRespone.Message, 400);

            var recommendedWorkoutsRespone = await mediator.Send(new GetRecommendedWorkoutsQuery(WorkoutIdsByPlanidRespone.Data, request.subcategoryName));
            if (!WorkoutIdsByPlanidRespone.IsSuccess)
                return RequestResponse<IEnumerable<RecomendedWorkoutsDto>>.Fail(recommendedWorkoutsRespone.Message, 400);

            return RequestResponse<IEnumerable<RecomendedWorkoutsDto>>.Success(recommendedWorkoutsRespone.Data, "Recommended workouts retrieved successfully");


        }
    }
}