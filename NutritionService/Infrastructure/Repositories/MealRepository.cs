using NutritionService.Domain.Entities;
using NutritionService.Infrastructure.Persistence.Data;
using NutritionService.Shared.Interfaces;

namespace NutritionService.Infrastructure.Repositories
{
    public class MealRepository : GenericRepository<Meal> , IMealRepository
    {
        public MealRepository(NutritionDbContext context) : base(context)
        {
        }
    }
}
