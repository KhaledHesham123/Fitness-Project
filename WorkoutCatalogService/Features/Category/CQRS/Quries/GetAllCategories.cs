using MediatR;
using Microsoft.EntityFrameworkCore;
using WorkoutCatalogService.Features.Category.DTOs;
using WorkoutCatalogService.Shared.Entites;
using WorkoutCatalogService.Shared.GenericRepos;
using WorkoutCatalogService.Shared.Response;

namespace WorkoutCatalogService.Features.Category.CQRS.Quries
{
    public record GetAllCategories:IRequest<RequestResponse<IEnumerable<CategoriesDTO>>>;

    public class GetAllCategoriesHandler : IRequestHandler<GetAllCategories, RequestResponse<IEnumerable<CategoriesDTO>>>
    {
        private readonly IGenericRepository<category> _categoryRepo;

        public GetAllCategoriesHandler(IGenericRepository<category> CategoryRepo)
        {
            _categoryRepo = CategoryRepo;
        }
        public async Task<RequestResponse<IEnumerable<CategoriesDTO>>> Handle(GetAllCategories request, CancellationToken cancellationToken)
        {
            var Categories = await  _categoryRepo.GetAll().Include(x=>x.SubCategories).ToListAsync();
            if (Categories==null)

                return RequestResponse<IEnumerable<CategoriesDTO>>.Fail("Cart is empty", 400);
            var mappedCategories = new List<CategoriesDTO>();

            foreach (var Category in Categories)
            {
                var subCategoriesDto = new List<SubCategoryDTo>();
                if (Category.SubCategories != null)
                {
                    foreach (var sc in Category.SubCategories)
                    {
                        subCategoriesDto.Add(new SubCategoryDTo
                        {
                            Id = sc.Id,
                            Name = sc.Name,
                            Description = sc.Description
                        });
                    }
                }

                var categoryDto = new CategoriesDTO
                {
                    id = Category.Id,
                    Name = Category.Name,
                    Description = Category.Description,
                    SubCategories = subCategoriesDto
                };

                mappedCategories.Add(categoryDto);
            }

            return RequestResponse<IEnumerable<CategoriesDTO>>.Success(mappedCategories);


        }
    }
}
