using System.ComponentModel.DataAnnotations;

namespace WorkoutCatalogService.Shared.Srvieces
{
    public static class DtoValidator<T>
    {
        public static bool TryValidate(object dto, out List<string> errors)
        {
            if (dto == null) 
            {
                errors = new List<string> { "The DTO object to validate cannot be null." };
                return false;
            }
            var validationResults = new List<ValidationResult>();
            var context = new ValidationContext(dto);

            bool isValid = System.ComponentModel.DataAnnotations.Validator
                       .TryValidateObject(dto, context, validationResults, true);
            errors = validationResults.Select(r => r.ErrorMessage!).ToList();
            return isValid;
        }
    }
}
