namespace WorkoutCatalogService.Shared.Constants
{
    public static class RabbitMQConstants
    {

        public const string PlanCreatedExchangeName = "Plan.events";

        public const string PlanCreatedRoutuigKey = "Plan.created";

        public const string WorkoutService_Plan_created_Queue = "workoutservice.plan.created.queue";
    }
}
