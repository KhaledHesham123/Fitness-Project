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
    public record CreateFullPlanOrchestrator(AddplanDto Plan, IEnumerable<AddPlanWorkoutDto?> PlanWorkouts, IEnumerable<WorkoutToaddDto?> Workouts) : IRequest<RequestResponse<Guid>>;

    public class CreateFullPlanOrchestratorHandler : IRequestHandler<CreateFullPlanOrchestrator, RequestResponse<Guid>>
    {
        private readonly IMediator _mediator;
        private readonly IConfiguration _configuration;
        private readonly IGenericRepository<Plan> genericRepository;

        public CreateFullPlanOrchestratorHandler(IMediator mediator, IConfiguration configuration, IGenericRepository<Plan> genericRepository)
        {
            this._mediator = mediator;
            this._configuration = configuration;
            this.genericRepository = genericRepository;
        }
        public async Task<RequestResponse<Guid>> Handle(CreateFullPlanOrchestrator request, CancellationToken cancellationToken)
        {
            try
            {
                var addplanResult = await _mediator.Send(new AddPlanCommend(request.Plan.id,request.Plan.Name, request.Plan.Description, request.Plan.DifficultyLevel, request.Plan.AssignedUserIds));
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







}


#region To do


//var planEntity = await genericRepository.GetByIdQueryable(planId).FirstAsync();
//planEntity.AssignedUserIds = users.Select(u => u.Id).ToList();
//genericRepository.SaveInclude(planEntity);
//await genericRepository.SaveChanges(); 





#endregion