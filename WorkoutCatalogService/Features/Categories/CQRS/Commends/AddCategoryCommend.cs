using MediatR;
using WorkoutCatalogService.Features.Categories.Controller;
using WorkoutCatalogService.Features.Categories.DTOs;
using WorkoutCatalogService.Shared.Entites;
using WorkoutCatalogService.Shared.GenericRepos;
using WorkoutCatalogService.Shared.Response;

namespace WorkoutCatalogService.Features.Categories.CQRS.Commends
{
    public record AddCategoryCommend(category category) :IRequest<RequestResponse<bool>>;

    public class AddCategoryCommendHandler: IRequestHandler<AddCategoryCommend, RequestResponse<bool>>
    {
        private readonly IGenericRepository<category> genericRepository;

        public AddCategoryCommendHandler(IGenericRepository<category> genericRepository)
        {
            this.genericRepository = genericRepository;
        }
        public async Task<RequestResponse<bool>> Handle(AddCategoryCommend request, CancellationToken cancellationToken)
        {
            if (request.category == null)
            {
                return RequestResponse<bool>.Fail("there is no category", 400);
            }
            var category = new category
            {
                Name = request.category.Name,
                Description = request.category.Description,
                SubCategories = request.category.SubCategories?
         .Select(sc => new SubCategory
         {
             Id = sc.Id,
             Name = sc.Name,
             Description = sc.Description
         })
         .ToList()
         ?? new List<SubCategory>()   
            };

            genericRepository.SaveInclude(category);
            await genericRepository.SaveChanges();

            return await Task.FromResult(RequestResponse<bool>.Success(true));
        }
    }



}
