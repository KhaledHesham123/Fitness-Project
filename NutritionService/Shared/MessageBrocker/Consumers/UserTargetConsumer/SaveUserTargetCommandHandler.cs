using MediatR;
using NutritionService.Domain.Entities;
using NutritionService.Shared.Interfaces;

namespace NutritionService.Shared.MessageBrocker.Consumers.UserTargetConsumer
{
    public class SaveUserTargetCommandHandler(IUserNutritionProfileRepository _nutritionProfileRepository):IRequestHandler<SaveUserTargetCommand>
    {
        async Task<Unit> IRequestHandler<SaveUserTargetCommand, Unit>.Handle(SaveUserTargetCommand request, CancellationToken cancellationToken)
        {
            Console.WriteLine("SaveUserTargetCommandHandler called");
            var exist = _nutritionProfileRepository.Get(n => n.UserId == request.UserId).FirstOrDefault();
            if (exist != null)
            {
                exist.DailyCalorieTarget = request.DailyCalorieTarget;
                exist.ProteinGoal = request.ProteinGoal;
                exist.CarbGoal = request.CarbGoal;
                exist.FatGoal = request.FatGoal;
                _nutritionProfileRepository.Update(exist);
                await _nutritionProfileRepository.SaveChangesAsync();
                return Unit.Value;
            }
            var userNutiritionProfile = new UserNutritionProfile
            {
                UserId = request.UserId,
                DailyCalorieTarget = request.DailyCalorieTarget,
                ProteinGoal = request.ProteinGoal,
                CarbGoal = request.CarbGoal,
                FatGoal = request.FatGoal
            };
            _nutritionProfileRepository.Add(userNutiritionProfile);
            await _nutritionProfileRepository.SaveChangesAsync();
            return Unit.Value;
        }
    }
}
