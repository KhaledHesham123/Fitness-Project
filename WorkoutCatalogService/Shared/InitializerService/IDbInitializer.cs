namespace WorkoutCatalogService.Shared.InitializerService
{
    public interface IDbInitializer
    {

        Task InitializeRabbitMQAsync();

    }
}