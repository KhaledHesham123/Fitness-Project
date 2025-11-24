using MediatR;
using Microsoft.IdentityModel.Tokens.Experimental;
using WorkoutCatalogService.Features.Categories.CQRS.Commends;
using WorkoutCatalogService.Features.Categories.CQRS.Quries;
using WorkoutCatalogService.Features.Categories.DTOs;
using WorkoutCatalogService.Shared.Entites;
using WorkoutCatalogService.Shared.Response;
using WorkoutCatalogService.Shared.Srvieces;

namespace WorkoutCatalogService.Features.Categories.CQRS.Orchestratots
{
    public record AddCategoryOrchestrator(string Name, string Description, ICollection<SubCategoryDTo> SubCategories) : IRequest<RequestResponse<bool>>;
    public class AddCategoryOrchestratorHandler : IRequestHandler<AddCategoryOrchestrator, RequestResponse<bool>>
    {
        private readonly IMediator mediator;

        public AddCategoryOrchestratorHandler(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<RequestResponse<bool>> Handle(AddCategoryOrchestrator request, CancellationToken cancellationToken)
        {


            var addCategoryRespone = await mediator.Send(new AddCategoryCommend(request.Name,request.Description));
           

            if (!addCategoryRespone.IsSuccess)
                return RequestResponse<bool>.Fail("Something went wrong during adding Category.", 400);

            var addsubcategoryRespone = await mediator.Send(new AddsubcategoriesCommend(addCategoryRespone.Data, request.SubCategories));
            if (!addsubcategoryRespone.IsSuccess)
                return RequestResponse<bool>.Fail("Something went wrong during adding SubCategories.", 400);

            return RequestResponse<bool>.Success(true, "Category and SubCategory added successfully", 201);

        }




    }


}
