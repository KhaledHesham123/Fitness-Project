using MediatR;
using System.Numerics;
using WorkoutCatalogService.Features.Categories.DTOs;
using WorkoutCatalogService.Features.Plans.DTOs;
using WorkoutCatalogService.Features.PlanWorkouts.DTOS;
using WorkoutCatalogService.Shared.Constants;
using WorkoutCatalogService.Shared.Entites;
using WorkoutCatalogService.Shared.GenericRepos;
using WorkoutCatalogService.Shared.MessageBrocker.MessageBrokerService;
using WorkoutCatalogService.Shared.MessageBrocker.MessageBrokerService.Messages;


using WorkoutCatalogService.Shared.Response;
using WorkoutCatalogService.Shared.Srvieces;

namespace WorkoutCatalogService.Features.Plans.CQRS.Commends
{
    public record AddPlanCommend(AddplanDto AddplanDto) : IRequest<RequestResponse<Guid>>;

    public class AddPlanCommendHandler : IRequestHandler<AddPlanCommend, RequestResponse<Guid>>
    {
        private readonly IGenericRepository<Shared.Entites.Plan> _genericRepository;
        private readonly IMessageBrokerPublisher messageBrokerPublisher;

        public AddPlanCommendHandler(IGenericRepository<Plan> genericRepository, IMessageBrokerPublisher messageBrokerPublisher)
        {
            this._genericRepository = genericRepository;
            this.messageBrokerPublisher = messageBrokerPublisher;
        }
        public async Task<RequestResponse<Guid>> Handle(AddPlanCommend request, CancellationToken cancellationToken)
        {

           
            try
            {
                var plan = new Plan
                {
                    Id = request.AddplanDto.id,
                    Description = request.AddplanDto.Description,
                    Name = request.AddplanDto.Name,
                    DifficultyLevel = Enum.Parse<DifficultyLevel>(request.AddplanDto.DifficultyLevel, true),
                    AssignedUserIds = request.AddplanDto.AssignedUserIds.ToList()
                };

                await _genericRepository.addAsync(plan);
                await _genericRepository.SaveChanges();

                var msg = new PlanAddedMessage
                {
                    Type = "PlanAdded",
                    Userid = request.AddplanDto.AssignedUserIds,
                    planid = plan.Id,
                };
                string TextMessage = System.Text.Json.JsonSerializer.Serialize(msg);

                await messageBrokerPublisher.PublishMessage(RabbitMQConstants.PlanCreatedExchangeName,
                                                              RabbitMQConstants.PlanCreatedRoutuigKey,
                                                              TextMessage);

                return RequestResponse<Guid>.Success(plan.Id, "Plan added successfully", 200);
            }
            catch (Exception ex)
            {
                return RequestResponse<Guid>.Fail($"Something went wrong during adding Plan: {ex.Message}", 500);
            }

        }
    }
}
