
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Threading.Tasks;
using WorkoutCatalogService.Data.Context;
using WorkoutCatalogService.Shared.GenericRepos;
using WorkoutCatalogService.Shared.InitializerService;
using WorkoutCatalogService.Shared.MessageBrocker.MessageBrokerService;
using WorkoutCatalogService.Shared.MiddleWares;
using WorkoutCatalogService.Shared.UnitofWorks;

namespace WorkoutCatalogService
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services.AddDbContext<WorkoutCatalogDbContext>(options => 
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("defaultConnection")).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });

            builder.Services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
            });
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            builder.Services.AddScoped<IunitofWork, UnitofWork>();


            builder.Services.AddSingleton<IMessageBrokerPublisher, MessageBrokerPublisher>();

            builder.Services.AddHostedService<RabbitMQConsumerService>();
            builder.Services.AddScoped<IDbInitializer, DbInitializer>();



            builder.Services.AddScoped<TransactionMiddlerWare>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseStaticFiles();

            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var _dbcontext = services.GetRequiredService<WorkoutCatalogDbContext>();
            var DbInitializer = services.GetRequiredService<IDbInitializer>();
            var logger= services.GetRequiredService<ILogger<Program>>();

            try
            {
                _dbcontext.Database.Migrate();
                await DbInitializer.InitializeRabbitMQAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An Error Occurred During Apply the Migration");

            }


            app.UseHttpsRedirection();

            app.UseMiddleware<TransactionMiddlerWare>();


            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
