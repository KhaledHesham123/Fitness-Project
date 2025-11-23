using MediatR;
using WorkoutCatalogService.Features.Categories.Controller;
using WorkoutCatalogService.Features.Categories.DTOs;
using WorkoutCatalogService.Shared.Entites;
using WorkoutCatalogService.Shared.GenericRepos;
using WorkoutCatalogService.Shared.Response;
using WorkoutCatalogService.Shared.Srvieces.Validation;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WorkoutCatalogService.Features.Categories.CQRS.Commends
{
    public record AddCategoryCommend(category category) :IRequest<RequestResponse<Guid>>;

    public class AddCategoryCommendHandler: IRequestHandler<AddCategoryCommend, RequestResponse<Guid>>
    {
        private readonly IGenericRepository<category> genericRepository;

        public AddCategoryCommendHandler(IGenericRepository<category> genericRepository)
        {
            this.genericRepository = genericRepository;
        }
        public async Task<RequestResponse<Guid>> Handle(AddCategoryCommend request, CancellationToken cancellationToken)
        {

            try
            {
                bool isValid = RequestValidator<category>.TryValidate(request.category, out List<string> errors);

                if (!isValid)
                {
                    return RequestResponse<Guid>.Fail(string.Join(", ", errors), 400);
                }
                var category = new category
                {
                    Name = request.category.Name,
                    Description = request.category.Description,
                };

                await genericRepository.addAsync(category);
                await genericRepository.SaveChanges();

                return RequestResponse<Guid>.Success(category.Id, "Category added successfully", 201);
            }
            catch (Exception ex)
            {
                return RequestResponse<Guid>.Fail(ex.Message, 400);


            }

        }
    }



}
