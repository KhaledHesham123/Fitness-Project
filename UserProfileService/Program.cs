
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using UserProfileService.Contract;
using UserProfileService.Data;
using UserProfileService.Features.Orchestrators;
using UserProfileService.Helper;
using UserProfileService.Shared.MiddleWares;
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
            builder.Services.AddHttpClient(); // يسجل HttpClient عادي
            builder.Services.AddScoped<IunitofWork, UnitofWork>();
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped<TransactionMiddlerWare>();
            builder.Services.AddScoped<IAddUserprofileQrcs, AddUserprofileQrcs>();
            builder.Services.AddScoped<IUpdateUserProfileQrccs, UpdateUserProfileQrccs>();
            builder.Services.AddScoped<IImageHelper, ImageHelper>();
            builder.Services.AddHttpContextAccessor();


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
