using Azure.Core;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens.Experimental;
using System.ComponentModel.DataAnnotations;
using WorkoutCatalogService.Features.Categories.DTOs;
using WorkoutCatalogService.Shared.Entites;
using WorkoutCatalogService.Shared.GenericRepos;
using WorkoutCatalogService.Shared.Response;
using WorkoutCatalogService.Shared.Srvieces.Validation;

namespace WorkoutCatalogService.Features.Categories.CQRS.Commends
{
    public record AddsubcategoriesCommend(Guid CategoryId, IEnumerable<SubCategoryDTo> SubCategoryDTos):IRequest<RequestResponse<IEnumerable<SubCategoryDTo>>>;

    public class AddsubcategoriesCommendHandler : IRequestHandler<AddsubcategoriesCommend, RequestResponse<IEnumerable<SubCategoryDTo>>>
    {
        private readonly IGenericRepository<SubCategory> genericRepository;

        public AddsubcategoriesCommendHandler(IGenericRepository<SubCategory> genericRepository)
        {
            this.genericRepository = genericRepository;
        }
        public async Task<RequestResponse<IEnumerable<SubCategoryDTo>>> Handle(AddsubcategoriesCommend request, CancellationToken cancellationToken)
        {
            var validationResponse = GetSubcategoriesValidationErrors(request.SubCategoryDTos);

            if (validationResponse.Any())
            {
                string errorMessage = string.Join(" , ", validationResponse);
                return RequestResponse<IEnumerable<SubCategoryDTo>>.Fail(errorMessage, 400);
            }

            var mappedSubcategories = request.SubCategoryDTos.Select(dto => new SubCategory
            {
                Description = dto.Description,
                Name = dto.Name,
                CategoryId = request.CategoryId,
                
            }).ToList();
            await genericRepository.AddRangeAsync(mappedSubcategories);
            await genericRepository.SaveChanges();

            return RequestResponse<IEnumerable<SubCategoryDTo>>.Success(request.SubCategoryDTos, "subcategories added successfully", 200);
        }

        private List<string> GetSubcategoriesValidationErrors(IEnumerable<SubCategoryDTo> subCategoryDTos)
        {

            return subCategoryDTos.SelectMany(dto =>
            {
                RequestValidator<SubCategoryDTo>.TryValidate(dto, out var dtoErrors);
                return dtoErrors.Select(e => $"Validation failed for DTO '{dto.Name}': {e}");
            }).ToList();



        }
    }




}
