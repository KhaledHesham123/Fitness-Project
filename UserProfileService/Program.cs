
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using UserProfileService.Data.Context;
using UserProfileService.Shared.GenericRepos;
using UserProfileService.Shared.MiddleWares;
using UserProfileService.Shared.Services.attachmentServices;
using UserProfileService.Shared.UnitofWorks;

namespace UserProfileService
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


            builder.Services.AddDbContext<UserProfileDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("defaultConnection")).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });

            builder.Services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
            });

            builder.Services.AddScoped<IunitofWork, UnitofWork>();

            builder.Services.AddScoped<IattachmentService,attachmentService>();



            builder.Services.AddScoped<TransactionMiddlerWare>();
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseStaticFiles();

            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var _dbcontext = services.GetRequiredService<UserProfileDbContext>();
            var logger = services.GetRequiredService<ILogger<Program>>();

            try
            {
                _dbcontext.Database.Migrate();
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
