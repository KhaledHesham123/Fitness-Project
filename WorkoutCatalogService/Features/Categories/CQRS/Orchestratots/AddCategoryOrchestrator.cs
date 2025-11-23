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
    public class AddCategoryOrchestratorHandler:IRequestHandler<AddCategoryOrchestrator, RequestResponse<bool>>
    {
        private readonly IMediator mediator;

        public AddCategoryOrchestratorHandler(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<RequestResponse<bool>> Handle(AddCategoryOrchestrator request, CancellationToken cancellationToken)
        {

            var ValidateRespone= ValidateOrchestratorRequest(request);

            if (ValidateRespone.Any())
            {
                var erros = string.Join(", ", ValidateRespone);
                return RequestResponse<bool>.Fail(erros, 400);
            }

            var addCategoryRespone= await mediator.Send(new AddCategoryCommend(new category
            {
              Name = request.Name,
              Description = request.Description,
            }));

               if (!addCategoryRespone.IsSuccess)
                    return RequestResponse<bool>.Fail("Something went wrong during adding Category.", 400);
    
               var addsubcategoryRespone = await mediator.Send(new AddsubcategoriesCommend(addCategoryRespone.Data, request.SubCategories));
                if (!addsubcategoryRespone.IsSuccess)
                     return RequestResponse<bool>.Fail("Something went wrong during adding SubCategories.", 400);

            return RequestResponse<bool>.Success(true,"Category and SubCategory added successfully",201);

        }

        
    

    private List<string> ValidateOrchestratorRequest(AddCategoryOrchestrator request)
        {
            var errors = new List<string>();

            
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                errors.Add("Category Name is required.");
            }
            if (string.IsNullOrWhiteSpace(request.Description))
            {
                errors.Add("Category Description is required.");
            }
            

            if (request.SubCategories == null || !request.SubCategories.Any())
            {
                errors.Add("At least one SubCategory is required.");
            }

            return errors;
        }
    }


}
