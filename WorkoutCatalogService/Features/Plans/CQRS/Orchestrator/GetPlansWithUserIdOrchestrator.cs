using MediatR;
using System.Net.Http;
using System.Text.Json;
using WorkoutCatalogService.Features.Plans.CQRS.Quries;
using WorkoutCatalogService.Features.Plans.DTOs;
using WorkoutCatalogService.Features.PlanWorkouts.DTOS;
using WorkoutCatalogService.Shared.Response;

namespace WorkoutCatalogService.Features.Plans.CQRS.Orchestrator
{
    public record GetPlansWithUserIdOrchestrator(Guid id) : IRequest<RequestResponse<IEnumerable<PalnToReturnDto>>>;

    public class GetPlansWithUserIdOrchestratorHandler : IRequestHandler<GetPlansWithUserIdOrchestrator, RequestResponse<IEnumerable<PalnToReturnDto>>>
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        public IMediator Mediator { get; }

        public GetPlansWithUserIdOrchestratorHandler(IMediator mediator, IHttpClientFactory httpClientFactory,
            IConfiguration configuration)
        {
            Mediator = mediator;
            this._httpClientFactory = httpClientFactory;
            this._configuration = configuration;
        }
        public async Task<RequestResponse<IEnumerable<PalnToReturnDto>>> Handle(GetPlansWithUserIdOrchestrator request, CancellationToken cancellationToken)
        {
            if (request.id == Guid.Empty)
            {
                var GetALlplansRespone = await Mediator.Send(new GetAllplansQuery());
                if (!GetALlplansRespone.IsSuccess)
                {
                    return RequestResponse<IEnumerable<PalnToReturnDto>>.Fail("error while geting plans", 400);
                }
                var PlanUsers=await GetUsersByPlanIds(GetALlplansRespone.Data.Select(p => p.id).Distinct());
                if (PlanUsers.Any())
                {
                    foreach (var plan in GetALlplansRespone.Data)
                    {
                        plan.UserName = PlanUsers.Where(u => u.PlanId == plan.id)
                            .Select(u => $"{u.FirstName} {u.LastName}").ToList();

                        plan.AssignedUserIds = PlanUsers.Where(PlanUsers => PlanUsers.PlanId == plan.id)
                            .Select(u => u.Id).ToList();

                    }
                }
               
               return RequestResponse<IEnumerable<PalnToReturnDto>>.Success(GetALlplansRespone.Data, "Plans retrieved successfully");


            }

            var planResponse = await Mediator.Send(new GetPlanbyidQyery(request.id));
            if (!planResponse.IsSuccess || planResponse.Data == null)
                return RequestResponse<IEnumerable<PalnToReturnDto>>.Fail("Plan not found", 404);

            var usersForPlan = await GetUsersByPlanIds(new List<Guid> { planResponse.Data.id });
            if (usersForPlan.Any())
            {
                var relatedUsers = usersForPlan.Where(u => u.PlanId == planResponse.Data.id);

                planResponse.Data.UserName = relatedUsers
                    .Select(u => $"{u.FirstName} {u.LastName}")
                    .ToList();

                planResponse.Data.AssignedUserIds = relatedUsers
                    .Select(u => u.Id)
                    .ToList();
            }

            return RequestResponse<IEnumerable<PalnToReturnDto>>.Success(new List<PalnToReturnDto> { planResponse.Data }, "Plans retrieved successfully");


        }
        public async Task<IEnumerable<UserToReturnDto>> GetUsersByPlanIds(IEnumerable<Guid> planids)
        {
            var httpclient = _httpClientFactory.CreateClient();
            var UserProfileServiceUrl = _configuration["Services:UserProfile"];



            var respone = await httpclient.PostAsJsonAsync($"{UserProfileServiceUrl}/UserProfile/GetUsersByPlanIds", planids);


            IEnumerable<UserToReturnDto> users = Enumerable.Empty<UserToReturnDto>();
            if (respone.IsSuccessStatusCode)
            {
                var content = await respone.Content.ReadAsStringAsync();
                users = JsonSerializer.Deserialize<IEnumerable<UserToReturnDto>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? Enumerable.Empty<UserToReturnDto>();
            }

            return users;
        }



    }
}










public class UserToReturnDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public decimal Weight { get; set; }
    public decimal Height { get; set; }
    public Guid? PlanId { get; set; }
}