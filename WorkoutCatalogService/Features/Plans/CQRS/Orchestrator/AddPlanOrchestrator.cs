using MediatR;
using System.Net.Http;
using System.Numerics;
using System.Text.Json;
using WorkoutCatalogService.Features.Plans.CQRS.Commends;

//using WorkoutCatalogService.Features.Plans.CQRS.Commends;
using WorkoutCatalogService.Features.Plans.DTOs;
using WorkoutCatalogService.Features.PlanWorkouts.CQRS.Commends;
using WorkoutCatalogService.Features.PlanWorkouts.DTOS;
using WorkoutCatalogService.Shared.Entites;
using WorkoutCatalogService.Shared.GenericRepos;
using WorkoutCatalogService.Shared.Response;
using WorkoutCatalogService.Shared.Srvieces;

namespace WorkoutCatalogService.Features.Plans.CQRS.Orchestrator
{
    public record AddPlanOrchestrator(AddplanDto AddplanDto, IEnumerable<AddPlanWorkoutDto> AddPlanWorkoutDto) : IRequest<RequestResponse<bool>>;

    public class AddPlanOrchestratorHandler : IRequestHandler<AddPlanOrchestrator, RequestResponse<bool>>
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
        public async Task<RequestResponse<bool>> Handle(AddPlanOrchestrator request, CancellationToken cancellationToken)
        {
            bool AddplanDto_isValid = DtoValidator<AddplanDto>.TryValidate(request.AddplanDto, out List<string> addPlanErrors);
            bool AddPlanWorkoutDto_isValid = DtoValidator<AddplanDto>.TryValidate(request.AddplanDto, out List<string> addPlanWorkoutErrors);


            if (!AddplanDto_isValid)
                return RequestResponse<bool>.Fail(string.Join(", ", addPlanErrors), 400);

            if (!AddPlanWorkoutDto_isValid)
                return RequestResponse<bool>.Fail(string.Join(", ", addPlanWorkoutErrors), 400);


            var planWorkouts = await _mediator.Send(new AddPlanWorkoutCommend(request.AddPlanWorkoutDto));

            var plan = await _mediator.Send(new AddPlanCommend(request.AddplanDto, planWorkouts));

            if (!plan.IsSuccess)
            {
                return RequestResponse<bool>.Fail("Something went wrong during adding Plan.", 400);


            }
            var UserProfileServiceUrl = _configuration["Services:UserProfile"];
            var httpclient = new HttpClient();

            var response = await httpclient.GetAsync(
                $"{UserProfileServiceUrl}/UserProfile/GetUsersbyplanid?id={plan.Data.Id}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var users = JsonSerializer.Deserialize<IEnumerable<UserToReturnDto>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });


                if (users != null && users.Any())
                {
                    foreach (var user in users)
                    {
                        plan.Data.AssignedUserIds.Add(user.Id);
                    }


                    genericRepository.SaveInclude(plan.Data);

                    await genericRepository.SaveChanges();

                }


            }
            return RequestResponse<bool>.Success(true, "Plan added successfully", 200);


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
