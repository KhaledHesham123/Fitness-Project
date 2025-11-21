using MediatR;
using WorkoutCatalogService.Features.Workout.DTOs;
using WorkoutCatalogService.Shared.Response;

namespace WorkoutCatalogService.Features.Workout.CQRS.Queries
{
    public record GetAllWorkoutsQuery:IRequest<RequestResponse<IEnumerable<WorkoutToreturnDto>>>;
    public class GetAllWorkoutsQueryHandler
    {
    }
}
