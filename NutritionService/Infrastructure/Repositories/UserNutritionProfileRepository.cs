using NutritionService.Domain.Entities;
using NutritionService.Infrastructure.Persistence.Data;
using NutritionService.Shared.Interfaces;

namespace NutritionService.Infrastructure.Repositories
{
    public class UserNutritionProfileRepository(NutritionDbContext _dbContext): GenericRepository<UserNutritionProfile>(_dbContext), IUserNutritionProfileRepository 
    {

    }
}
