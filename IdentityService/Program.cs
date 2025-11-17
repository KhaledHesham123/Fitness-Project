using FluentValidation;
using IdentityService.Authorization;
using IdentityService.Data.DBContexts;
using IdentityService.Data.Seeders;
using IdentityService.Features.Authantication.Commands.Register;
using IdentityService.Features.Shared;
using IdentityService.Features.Shared.CheckExist;
using IdentityService.Shared.Behaviors;
using IdentityService.Shared.Entities;
using IdentityService.Shared.Interfaces;
using IdentityService.Shared.Middlewares;
using IdentityService.Shared.Repositories;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace IdentityService
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddControllers();
            builder.Services.AddOpenApi();

            builder.Services.AddDbContext<IdentityDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityDatabase"));
            });

            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddValidatorsFromAssembly(typeof(RegisterCommand).Assembly);
            builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            builder.Services.AddMediatR(typeof(Program).Assembly);

            builder.Services.AddCors(option => option.AddPolicy("myPolicy", option =>
            {
                option.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            }));

            #region Register CheckExistQueryHandlers dynamically
            // types that should support CheckExistQuery<T>
            var entityAssembly = typeof(User).Assembly; // adjust if entities in different assembly
            var entityTypes = entityAssembly.GetTypes()
                .Where(t => typeof(BaseEntity).IsAssignableFrom(t) && !t.IsAbstract && t.IsClass)
                .ToList();

            var handlerOpenType = typeof(CheckExistQueryHandler<>);
            var serviceOpenType = typeof(IRequestHandler<,>);
            var queryOpenType = typeof(CheckExistQuery<>);
            var responseType = typeof(Result<bool>);

            foreach (var entityType in entityTypes)
            {
                // service: IRequestHandler< CheckExistQuery<entityType>, ResponseResult<bool> >
                var serviceType = serviceOpenType.MakeGenericType(queryOpenType.MakeGenericType(entityType), responseType);

                // implementation: CheckExistQueryHandler<entityType>
                var implType = handlerOpenType.MakeGenericType(entityType);

                builder.Services.AddTransient(serviceType, implType);
            }
            #endregion


            builder.Services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
               .AddJwtBearer(options =>
               {
                   var issuer = builder.Configuration["JWT:Author"];
                   var Audience = builder.Configuration["JWT:Audience"];
                   var key = builder.Configuration["JWT:Key"];
                   options.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateIssuer = true,
                       ValidateAudience = true,
                       ValidateLifetime = true,
                       ValidateIssuerSigningKey = true,
                       ValidIssuer = issuer,
                       ValidAudience = Audience,
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
                   };
               });

            builder.Services.AddAuthorization();

            builder.Services.AddMemoryCache();

            // Dynamic RBAC
            builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
            builder.Services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();

            var app = builder.Build();

            app.UseMiddleware<ExceptionHandlingMiddleware>();

            // Seed
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<IdentityDbContext>();
                    await DataSeeder.SeedAsync(context);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while seeding the database.");
                }
            }
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
