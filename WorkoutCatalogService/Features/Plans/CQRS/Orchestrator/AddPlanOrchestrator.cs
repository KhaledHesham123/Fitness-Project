using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System.Numerics;
using System.Text.Json;
using WorkoutCatalogService.Features.Plans.CQRS.Commends;

//using WorkoutCatalogService.Features.Plans.CQRS.Commends;
using WorkoutCatalogService.Features.Plans.DTOs;
using WorkoutCatalogService.Features.PlanWorkouts.CQRS.Commends;
using WorkoutCatalogService.Features.PlanWorkouts.DTOS;
using WorkoutCatalogService.Features.Workout.CQRS.Commend;
using WorkoutCatalogService.Features.Workout.DTOs;
using WorkoutCatalogService.Shared.Entites;
using WorkoutCatalogService.Shared.GenericRepos;
using WorkoutCatalogService.Shared.Response;
using WorkoutCatalogService.Shared.Srvieces;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WorkoutCatalogService.Features.Plans.CQRS.Orchestrator
{
    public record AddPlanOrchestrator(AddplanDto Plan, IEnumerable<AddPlanWorkoutDto?> PlanWorkouts, IEnumerable<WorkoutToaddDto?> Workouts) : IRequest<RequestResponse<Guid>>;

    public class AddPlanOrchestratorHandler : IRequestHandler<AddPlanOrchestrator, RequestResponse<Guid>>
    {
        private readonly IMediator _mediator;
        private readonly IConfiguration _configuration;
        private readonly IGenericRepository<Plan> genericRepository;

        public AddPlanOrchestratorHandler(IMediator mediator, IConfiguration configuration, IGenericRepository<Plan> genericRepository)
        {
            this._mediator = mediator;
            this._configuration = configuration;
            this.genericRepository = genericRepository;
        }
        public async Task<RequestResponse<Guid>> Handle(AddPlanOrchestrator request, CancellationToken cancellationToken)
        {

            try
            {
                var addplanResult = await _mediator.Send(new AddPlanCommend(request.Plan));
                if (!addplanResult.IsSuccess)
                    return RequestResponse<Guid>.Fail(addplanResult.Message, 400);

                var planId = addplanResult.Data;

                if (request.Workouts != null && request.Workouts.Any())
                {
                    await _mediator.Send(new AddWorkoutsCommend(request.Workouts));
                }

                if (request.PlanWorkouts != null && request.PlanWorkouts.Any())
                {
                    var planWorkouts = request.PlanWorkouts.Select(pw =>
                    {
                        pw.WorkoutPlanId = planId; 
                        return pw;
                    });

                    await _mediator.Send(new AddPlanWorkoutCommend(planWorkouts));
                }

               

                return RequestResponse<Guid>.Success(planId, "Full plan created successfully", 201);


            }
            catch (Exception)
            {

                throw;
            }

        }
    }




    public class UserToReturnDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? ProfilePictureUrl { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public decimal Weight { get; set; }
        public decimal Height { get; set; }
        public string FitnessGoal { get; set; }
        public Guid? PlanId { get; set; }
    }


}


#region To do
//var UserProfileServiceUrl = _configuration["Services:UserProfile"];
//var httpclient = new HttpClient();

//var response = await httpclient.GetAsync(
//    $"{UserProfileServiceUrl}/UserProfile/GetUsersbyplanid?id={planId}");

//IEnumerable<UserToReturnDto> users = Enumerable.Empty<UserToReturnDto>();
//if (response.IsSuccessStatusCode)
//{
//    var content = await response.Content.ReadAsStringAsync();
//    users = JsonSerializer.Deserialize<IEnumerable<UserToReturnDto>>(content, new JsonSerializerOptions
//    {
//        PropertyNameCaseInsensitive = true
//    }) ?? Enumerable.Empty<UserToReturnDto>();
//}

//var planEntity = await genericRepository.GetByIdQueryable(planId).FirstAsync();
//planEntity.AssignedUserIds = users.Select(u => u.Id).ToList();
//genericRepository.SaveInclude(planEntity);
//await genericRepository.SaveChanges(); 
#endregion