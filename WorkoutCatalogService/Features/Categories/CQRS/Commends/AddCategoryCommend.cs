using MediatR;
using Microsoft.Extensions.Caching.Memory;
using WorkoutCatalogService.Features.Categories.Controller;
using WorkoutCatalogService.Features.Categories.DTOs;
using WorkoutCatalogService.Shared.Entites;
using WorkoutCatalogService.Shared.GenericRepos;
using WorkoutCatalogService.Shared.Response;
using WorkoutCatalogService.Shared.Srvieces;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WorkoutCatalogService.Features.Categories.CQRS.Commends
{
    public record AddCategoryCommend(string Name, string Description) :IRequest<RequestResponse<Guid>>;

    public class AddCategoryCommendHandler: IRequestHandler<AddCategoryCommend, RequestResponse<Guid>>
    {
        private readonly IGenericRepository<category> genericRepository;

        public AddCategoryCommendHandler(IGenericRepository<category> genericRepository, IMemoryCache memoryCache)
        {
            this.genericRepository = genericRepository;
            MemoryCache = memoryCache;
        }

        public IMemoryCache MemoryCache { get; }

        public async Task<RequestResponse<Guid>> Handle(AddCategoryCommend request, CancellationToken cancellationToken)
        {

            try
            {

                //var existingCategory = await genericRepository.GetByCriteriaAsync(c => c.Name == request.Name);
                //if (existingCategory!=null)
                //{
                //    return RequestResponse<Guid>.Fail("Category with this name already exists", 400);
                //}

                var category = new category
                {
                    Name = request.Name,
                    Description = request.Description,
                };

                await genericRepository.addAsync(category);
                await genericRepository.SaveChanges();

                MemoryCache.Set(
                   category.Id,          
                   new CategoriesDTO                  
                   {
                       Name = category.Name,
                       Description = category.Description
                   },
                   TimeSpan.FromHours(2)              
               );
                return RequestResponse<Guid>.Success(category.Id, "Category added successfully", 201);
            }
            catch (Exception ex)
            {
                return RequestResponse<Guid>.Fail(ex.Message, 400);


            }

        }
    }



}
