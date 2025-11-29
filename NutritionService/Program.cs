using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using NutritionService.Infrastructure.Persistence.Data;
using NutritionService.Infrastructure.Repositories;
using NutritionService.Shared.Interfaces;
using NutritionService.Shared.MessageBrocker.Consumers.UserTargetConsumer;
using NutritionService.Shared.MessageBrocker.MessageBrokerService;
using System.Reflection;

namespace NutritionService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddOpenApi();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            //Add Swagger 
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Nutrition Service API",
                    Version = "v1",
                    Description = "API for Meal Recommendations and Nutrition Management"
                });
            });
            builder.Services.AddDbContext<NutritionDbContext>(options =>
                   options.UseSqlServer(builder.Configuration.GetConnectionString("NutritionDatabase"), sqlServerOptionsAction: sqlOptions =>
                   {
                       // Add the retry configuration here
                       sqlOptions.EnableRetryOnFailure(
                           maxRetryCount: 5,                  
                           maxRetryDelay: TimeSpan.FromSeconds(30), 
                           errorNumbersToAdd: null            // Use the default set of transient error codes
                       );
                   }
               )
            );          
            builder.Services.AddMediatR(typeof(Program).Assembly, typeof(SaveUserTargetCommandHandler).Assembly);
            builder.Services.AddScoped(typeof(IGenericRepository<>),typeof(GenericRepository<>));
            builder.Services.AddScoped<IMealRepository,MealRepository>();
            builder.Services.AddScoped<IUserNutritionProfileRepository,UserNutritionProfileRepository>();
            builder.Services.AddScoped<IMessageBrokerPublisher, MessageBrokerPublisher>();
            builder.Services.AddHostedService<RabbitMQConsumerService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            // Enable Swagger middleware
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Nutrition Service API v1");
                    c.RoutePrefix = string.Empty; // Swagger at root URL
                });
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            try
            {
                app.MapControllers();
            }
            catch (ReflectionTypeLoadException ex)
            {
                foreach (var loaderEx in ex.LoaderExceptions)
                {
                    Console.WriteLine(loaderEx.Message);
                }
                throw;
            }

            app.Run();
        }
    }
}
