
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using WorkoutCatalogService.Data.Context;

namespace WorkoutCatalogService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services.AddDbContext<WorkoutCatalogDbContext>(options => 
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("defaultConnection"));
            });

            builder.Services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
            });
            builder.Services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
            });
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
            var logger= services.GetRequiredService<ILogger<Program>>();

            try
            {
                _dbcontext.Database.Migrate();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An Error Occurred During Apply the Migration");

            }


            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
