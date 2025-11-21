using MediatR;
using WorkoutCatalogService.Features.Categories.CQRS.Commends;
using WorkoutCatalogService.Features.Categories.CQRS.Quries;
using WorkoutCatalogService.Features.Categories.DTOs;
using WorkoutCatalogService.Shared.Entites;
using WorkoutCatalogService.Shared.Response;

namespace WorkoutCatalogService.Features.Categories.CQRS.Orchestratots
{
    public record AddCategoryOrchestrator(CategoryToaddDTO CategoryToaddDTO): IRequest<RequestResponse<bool>>;
    public class AddCategoryOrchestratorHandler:IRequestHandler<AddCategoryOrchestrator, RequestResponse<bool>>
    {
        private readonly IMediator mediator;

        public AddCategoryOrchestratorHandler(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<RequestResponse<bool>> Handle(AddCategoryOrchestrator request, CancellationToken cancellationToken)
        {

            if (request.CategoryToaddDTO == null)
            {
                return (RequestResponse<bool>.Fail("Category data is null", 400));
            }

            var subcategoryIds = request.CategoryToaddDTO.SubCategories.Select(x => x.id).ToList();

            var subcategories = await mediator.Send(new GetAllSubcategoryByidQuery(subcategoryIds));

            var category = new category
            {
                Name = request.CategoryToaddDTO.Name,
                Description = request.CategoryToaddDTO.Description,
            };
            if (subcategories.IsSuccess) 
            {
               category.SubCategories = subcategories.Data.ToList();

            }

            var result = await mediator.Send(new AddCategoryCommend(category));

            if (!result.IsSuccess)
            {
                return RequestResponse<bool>.Fail("Failed to add category", 400);
            }

            return RequestResponse<bool>.Success(true);

        }
    }
}
