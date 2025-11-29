using MediatR;

namespace NutritionService.Shared.MessageBrocker.Consumers.UserTargetConsumer
{
    public record SaveUserTargetCommand(int UserId,int DailyCalorieTarget,double ProteinGoal,double CarbGoal, double FatGoal) : IRequest;

}